namespace FSI.SmartPark.Domain.Interfaces
{
    /// <summary>
    /// Contrato genérico do Generic Repository Pattern para o SmartPark.
    /// Todos os repositórios concretos implementam esta interface via RepositoryBase{TEntity}.
    ///
    /// Convenções:
    ///   - Delete utiliza Soft Delete (IsDeleted = true) — nunca remove físicamente.
    ///   - GetAll e GetAllByEmpresa filtram automaticamente registros com IsDeleted = true.
    ///   - CancellationToken é propagado em todos os métodos async.
    /// </summary>
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        // ─── Command (escrita) ─────────────────────────────────────────────────

        /// <summary>Persiste uma nova entidade e retorna o Id gerado.</summary>
        Task<int> Add(TEntity entity, CancellationToken ct = default);

        /// <summary>Atualiza os dados de uma entidade existente.</summary>
        Task<bool> Update(TEntity entity, CancellationToken ct = default);

        /// <summary>
        /// Exclusão lógica (Soft Delete): marca IsDeleted = true e registra DeletedAt.
        /// O dado permanece no banco para histórico e auditoria.
        /// </summary>
        Task<bool> Delete(int id, CancellationToken ct = default);

        // ─── Query (leitura) ───────────────────────────────────────────────────

        /// <summary>Retorna a entidade pelo Id, ou null se não encontrada ou excluída.</summary>
        Task<TEntity?> GetById(int id, CancellationToken ct = default);

        /// <summary>Retorna todos os registros ativos (IsDeleted = false).</summary>
        Task<IEnumerable<TEntity>> GetAll(CancellationToken ct = default);

        /// <summary>
        /// Retorna todos os registros ativos (IsDeleted = false) pertencentes a uma empresa.
        /// Usado para garantir isolamento multi-tenant nas consultas.
        /// </summary>
        Task<IEnumerable<TEntity>> GetAllByEmpresa(int empresaId, CancellationToken ct = default);
    }
}
