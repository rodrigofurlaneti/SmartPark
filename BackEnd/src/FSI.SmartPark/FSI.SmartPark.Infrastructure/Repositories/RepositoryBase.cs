using Dapper;
using FSI.SmartPark.Domain.Interfaces;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories;

public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
    where TEntity : class
{
    protected readonly SmartParkDbContext _ctx;
    protected abstract string Tabela { get; }

    protected RepositoryBase(SmartParkDbContext ctx) => _ctx = ctx;

    public virtual async Task<int> Inserir(TEntity entity)
    {
        using var conn = _ctx.CreateConnection();
        try
        {
            var props = typeof(TEntity)
                .GetProperties()
                .Where(p =>
                    p.Name != "Id" &&
                    p.Name != "DataInsercao" &&
                    p.CanWrite &&
                    p.CanRead &&
                    !(p.PropertyType == typeof(DateTime) &&
                      (DateTime)p.GetValue(entity)! == DateTime.MinValue) &&
                    !(p.PropertyType == typeof(DateTime?) &&
                      ((DateTime?)p.GetValue(entity) is null ||
                       (DateTime?)p.GetValue(entity) == DateTime.MinValue)) &&
                    p.GetValue(entity) != null
                )
                .ToList();

            if (!props.Any())
                throw new InvalidOperationException($"Nenhuma propriedade válida para inserção em {Tabela}.");

            // ✅ DynamicParameters — passa só os campos filtrados
            var parameters = new DynamicParameters();
            foreach (var p in props)
                parameters.Add(p.Name, p.GetValue(entity));

            var colunas = string.Join(", ", props.Select(p => p.Name));
            var params_ = string.Join(", ", props.Select(p => $"@{p.Name}"));
            var sql = $"INSERT INTO SmartPark.{Tabela} ({colunas}) VALUES ({params_}); SELECT LAST_INSERT_ID();";

            Console.WriteLine($"[SQL] Inserir em {Tabela}: {sql}");
            foreach (var p in props)
                Console.WriteLine($"[SQL]   {p.Name} = {p.GetValue(entity)}");

            var id = await conn.ExecuteScalarAsync<int>(sql, parameters); // ✅ passa parameters

            if (id <= 0)
                throw new InvalidOperationException($"Inserção em {Tabela} não retornou Id válido.");

            Console.WriteLine($"[SQL] Inserido com Id = {id}");
            return id;
        }
        catch (MySqlConnector.MySqlException ex)
        {
            Console.WriteLine($"[ERRO SQL] Tabela: {Tabela} | Código: {ex.ErrorCode} | {ex.Message}");
            var mensagem = ex.ErrorCode switch
            {
                MySqlConnector.MySqlErrorCode.DuplicateKeyEntry => $"Registro duplicado em {Tabela}.",
                MySqlConnector.MySqlErrorCode.NoReferencedRow2 => $"Chave estrangeira inválida em {Tabela}.",
                MySqlConnector.MySqlErrorCode.ColumnCannotBeNull => $"Campo obrigatório nulo em {Tabela}.",
                MySqlConnector.MySqlErrorCode.DataTooLong => $"Valor muito longo em {Tabela}.",
                _ => $"Erro de banco em {Tabela}: {ex.Message}"
            };
            throw new InvalidOperationException(mensagem, ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERRO] Inserir - Tabela: {Tabela} | {ex.Message}");
            throw;
        }
    }

    public virtual async Task<bool> Excluir(int id)
    {
        try
        {
            if (id <= 0) throw new ArgumentException($"Id inválido: {id}");

            using var conn = _ctx.CreateConnection();
            var rows = await conn.ExecuteAsync(
                $"DELETE FROM SmartPark.{Tabela} WHERE Id = @id", new { id });

            if (rows == 0)
                throw new KeyNotFoundException($"{typeof(TEntity).Name} com Id {id} não encontrado para exclusão.");

            Console.WriteLine($"[SQL] Excluir - {Tabela} | Id = {id} | OK");
            return true;
        }
        catch (KeyNotFoundException) { throw; }
        catch (MySqlConnector.MySqlException ex)
        {
            Console.WriteLine($"[ERRO SQL] Excluir - Tabela: {Tabela} | Id: {id} | {ex.ErrorCode} | {ex.Message}");
            throw new InvalidOperationException($"Erro ao excluir {typeof(TEntity).Name}: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERRO] Excluir - Tabela: {Tabela} | Id: {id} | {ex.Message}");
            throw;
        }
    }

    public virtual async Task<bool> Atualizar(TEntity entity)
    {
        try
        {
            using var conn = _ctx.CreateConnection();
            var props = typeof(TEntity)
                .GetProperties()
                .Where(p => p.Name != "Id" && p.Name != "DataInsercao" && p.CanWrite && p.CanRead)
                .ToList();

            var sets = string.Join(", ", props.Select(p => $"{p.Name} = @{p.Name}"));
            var sql = $"UPDATE SmartPark.{Tabela} SET {sets} WHERE Id = @Id";

            Console.WriteLine($"[SQL] Atualizar - {Tabela}: {sql}");

            var rows = await conn.ExecuteAsync(sql, entity);

            if (rows == 0)
                throw new KeyNotFoundException($"{typeof(TEntity).Name} não encontrado para atualização.");

            return true;
        }
        catch (KeyNotFoundException) { throw; }
        catch (MySqlConnector.MySqlException ex)
        {
            Console.WriteLine($"[ERRO SQL] Atualizar - Tabela: {Tabela} | {ex.ErrorCode} | {ex.Message}");

            var mensagem = ex.ErrorCode switch
            {
                MySqlConnector.MySqlErrorCode.DuplicateKeyEntry => $"Registro duplicado em {Tabela}.",
                MySqlConnector.MySqlErrorCode.NoReferencedRow2 => $"Chave estrangeira inválida em {Tabela}.",
                MySqlConnector.MySqlErrorCode.ColumnCannotBeNull => $"Campo obrigatório nulo em {Tabela}.",
                _ => $"Erro ao atualizar {Tabela}: {ex.Message}"
            };

            throw new InvalidOperationException(mensagem, ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERRO] Atualizar - Tabela: {Tabela} | {ex.Message}");
            throw;
        }
    }

    public virtual async Task<TEntity> ObterPorId(int id)
    {
        try
        {
            if (id <= 0)
                throw new ArgumentException($"Id inválido: {id}");

            using var conn = _ctx.CreateConnection();
            var resultado = await conn.QueryFirstOrDefaultAsync<TEntity>(
                $"SELECT * FROM SmartPark.{Tabela} WHERE Id = @id", new { id });

            if (resultado is null)
                throw new KeyNotFoundException($"{typeof(TEntity).Name} com Id {id} não encontrado em {Tabela}.");

            Console.WriteLine($"[SQL] ObterPorId - {Tabela} | Id = {id} | Encontrado");
            return resultado;
        }
        catch (KeyNotFoundException)
        {
            throw; // repassa sem logar — é esperado
        }
        catch (MySqlConnector.MySqlException ex)
        {
            Console.WriteLine($"[ERRO SQL] ObterPorId - Tabela: {Tabela} | Id: {id} | Código: {ex.ErrorCode} | {ex.Message}");
            throw new InvalidOperationException($"Erro ao buscar {typeof(TEntity).Name} com Id {id}: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERRO] ObterPorId - Tabela: {Tabela} | Id: {id} | {ex.Message}");
            throw;
        }
    }

    public virtual async Task<IEnumerable<TEntity>> ListarTodos()
    {
        try
        {
            using var conn = _ctx.CreateConnection();
            var resultado = await conn.QueryAsync<TEntity>($"SELECT * FROM SmartPark.{Tabela}");
            return resultado;
        }
        catch (Exception ex)
        {
            // Troque pelo seu logger
            Console.WriteLine($"[ERRO] ListarTodos - Tabela: {Tabela} | {ex.Message}");
            throw;
        }
    }
}
