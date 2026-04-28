using Dapper;
using FSI.SmartPark.Domain.Entities;
using FSI.SmartPark.Domain.Interfaces;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories;

/// <summary>
/// Implementação genérica do Generic Repository Pattern com Dapper + MySQL.
///
/// Padrões aplicados:
///   ✅ Soft Delete — Delete() atualiza IsDeleted=1 e DeletedAt; nunca remove o registro.
///   ✅ Filtro automático — GetById/GetAll/GetAllByEmpresa ignoram IsDeleted=1.
///   ✅ Async/Await — CancellationToken propagado em todas as operações.
///   ✅ Isolamento multi-tenant — GetAllByEmpresa filtra por Empresa_Id.
/// </summary>
public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
    where TEntity : class
{
    protected readonly SmartParkDbContext _ctx;

    /// <summary>Nome da tabela no schema SmartPark (ex: "Unidade", "Cliente").</summary>
    protected abstract string Tabela { get; }

    protected RepositoryBase(SmartParkDbContext ctx) => _ctx = ctx;

    // ═══════════════════════════════════════════════════════════════════════════
    //  ADD  — INSERT INTO
    // ═══════════════════════════════════════════════════════════════════════════

    public virtual async Task<int> Add(TEntity entity, CancellationToken ct = default)
    {
        using var conn = _ctx.CreateConnection();
        try
        {
            var props = typeof(TEntity)
                .GetProperties()
                .Where(p =>
                    p.Name != "Id" &&
                    p.Name != "DataInsercao" &&
                    p.CanWrite && p.CanRead &&
                    p.GetValue(entity) != null &&
                    !(p.PropertyType == typeof(DateTime) &&
                      (DateTime)p.GetValue(entity)! == DateTime.MinValue) &&
                    !(p.PropertyType == typeof(DateTime?) &&
                      ((DateTime?)p.GetValue(entity)) == DateTime.MinValue))
                .ToList();

            if (!props.Any())
                throw new InvalidOperationException($"Nenhuma propriedade válida para inserção em {Tabela}.");

            var parameters = new DynamicParameters();
            foreach (var p in props)
                parameters.Add(p.Name, p.GetValue(entity));

            var colunas = string.Join(", ", props.Select(p => p.Name));
            var valores = string.Join(", ", props.Select(p => $"@{p.Name}"));
            var sql     = $"INSERT INTO {Tabela} ({colunas}) VALUES ({valores}); SELECT LAST_INSERT_ID();";

            var id = await conn.ExecuteScalarAsync<int>(new CommandDefinition(sql, parameters, cancellationToken: ct));

            if (id <= 0)
                throw new InvalidOperationException($"Inserção em {Tabela} não retornou Id válido.");

            return id;
        }
        catch (MySqlConnector.MySqlException ex)
        {
            var msg = ex.ErrorCode switch
            {
                MySqlConnector.MySqlErrorCode.DuplicateKeyEntry  => $"Registro duplicado em {Tabela}.",
                MySqlConnector.MySqlErrorCode.NoReferencedRow2   => $"Chave estrangeira inválida em {Tabela}.",
                MySqlConnector.MySqlErrorCode.ColumnCannotBeNull => $"Campo obrigatório nulo em {Tabela}.",
                MySqlConnector.MySqlErrorCode.DataTooLong        => $"Valor muito longo em {Tabela}.",
                _                                                 => $"Erro de banco em {Tabela}: {ex.Message}"
            };
            throw new InvalidOperationException(msg, ex);
        }
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  UPDATE  — UPDATE SET
    // ═══════════════════════════════════════════════════════════════════════════

    public virtual async Task<bool> Update(TEntity entity, CancellationToken ct = default)
    {
        using var conn = _ctx.CreateConnection();
        try
        {
            var props = typeof(TEntity)
                .GetProperties()
                .Where(p => p.Name != "Id" && p.Name != "DataInsercao" && p.CanWrite && p.CanRead)
                .ToList();

            var sets = string.Join(", ", props.Select(p => $"{p.Name} = @{p.Name}"));
            var sql  = $"UPDATE {Tabela} SET {sets} WHERE Id = @Id AND IsDeleted = 0";

            var rows = await conn.ExecuteAsync(new CommandDefinition(sql, entity, cancellationToken: ct));

            if (rows == 0)
                throw new KeyNotFoundException($"{typeof(TEntity).Name} não encontrado ou já excluído.");

            return true;
        }
        catch (KeyNotFoundException) { throw; }
        catch (MySqlConnector.MySqlException ex)
        {
            var msg = ex.ErrorCode switch
            {
                MySqlConnector.MySqlErrorCode.DuplicateKeyEntry  => $"Registro duplicado em {Tabela}.",
                MySqlConnector.MySqlErrorCode.NoReferencedRow2   => $"Chave estrangeira inválida em {Tabela}.",
                MySqlConnector.MySqlErrorCode.ColumnCannotBeNull => $"Campo obrigatório nulo em {Tabela}.",
                _                                                 => $"Erro ao atualizar {Tabela}: {ex.Message}"
            };
            throw new InvalidOperationException(msg, ex);
        }
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  DELETE  — SOFT DELETE (IsDeleted = 1, DeletedAt = UTC_TIMESTAMP())
    //  ❌ Nunca executa DELETE físico — apenas marca o registro como inativo.
    // ═══════════════════════════════════════════════════════════════════════════

    public virtual async Task<bool> Delete(int id, CancellationToken ct = default)
    {
        if (id <= 0) throw new ArgumentException($"Id inválido: {id}", nameof(id));

        using var conn = _ctx.CreateConnection();
        try
        {
            const string sql = @"
                UPDATE {0}
                   SET IsDeleted = 1,
                       DeletedAt = UTC_TIMESTAMP()
                 WHERE Id = @id
                   AND IsDeleted = 0";

            var rows = await conn.ExecuteAsync(
                new CommandDefinition(string.Format(sql, Tabela), new { id }, cancellationToken: ct));

            if (rows == 0)
                throw new KeyNotFoundException(
                    $"{typeof(TEntity).Name} com Id {id} não encontrado ou já excluído em {Tabela}.");

            return true;
        }
        catch (KeyNotFoundException) { throw; }
        catch (MySqlConnector.MySqlException ex)
        {
            throw new InvalidOperationException(
                $"Erro ao excluir (soft delete) {typeof(TEntity).Name} Id {id}: {ex.Message}", ex);
        }
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  GET BY ID  — filtra IsDeleted = 0
    // ═══════════════════════════════════════════════════════════════════════════

    public virtual async Task<TEntity?> GetById(int id, CancellationToken ct = default)
    {
        if (id <= 0) throw new ArgumentException($"Id inválido: {id}", nameof(id));

        using var conn = _ctx.CreateConnection();
        try
        {
            var sql = $"SELECT * FROM {Tabela} WHERE Id = @id AND IsDeleted = 0";

            return await conn.QueryFirstOrDefaultAsync<TEntity>(
                new CommandDefinition(sql, new { id }, cancellationToken: ct));
        }
        catch (MySqlConnector.MySqlException ex)
        {
            throw new InvalidOperationException(
                $"Erro ao buscar {typeof(TEntity).Name} Id {id} em {Tabela}: {ex.Message}", ex);
        }
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  GET ALL  — filtra IsDeleted = 0
    // ═══════════════════════════════════════════════════════════════════════════

    public virtual async Task<IEnumerable<TEntity>> GetAll(CancellationToken ct = default)
    {
        using var conn = _ctx.CreateConnection();
        try
        {
            var sql = $"SELECT * FROM {Tabela} WHERE IsDeleted = 0";

            return await conn.QueryAsync<TEntity>(
                new CommandDefinition(sql, cancellationToken: ct));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao listar {Tabela}: {ex.Message}", ex);
        }
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  GET ALL BY EMPRESA  — isolamento multi-tenant + IsDeleted = 0
    // ═══════════════════════════════════════════════════════════════════════════

    public virtual async Task<IEnumerable<TEntity>> GetAllByEmpresa(int empresaId, CancellationToken ct = default)
    {
        if (empresaId <= 0) throw new ArgumentException($"EmpresaId inválido: {empresaId}", nameof(empresaId));

        using var conn = _ctx.CreateConnection();
        try
        {
            var sql = $"SELECT * FROM {Tabela} WHERE Empresa_Id = @empresaId AND IsDeleted = 0";

            return await conn.QueryAsync<TEntity>(
                new CommandDefinition(sql, new { empresaId }, cancellationToken: ct));
        }
        catch (MySqlConnector.MySqlException ex)
        {
            throw new InvalidOperationException(
                $"Erro ao listar {Tabela} por EmpresaId {empresaId}: {ex.Message}", ex);
        }
    }
}
