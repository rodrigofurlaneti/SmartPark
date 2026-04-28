namespace FSI.SmartPark.Domain.Entities
{
    /// <summary>
    /// Classe base para todas as entidades do sistema SmartPark.
    /// Aplica encapsulamento, identidade por ID, auditoria de inserção e soft delete.
    /// </summary>
    public abstract class EntityBase
    {
        /// <summary>
        /// Identificador único gerado pelo banco (Auto-increment).
        /// Protected set impede alteração manual pelo Application Layer.
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// Data e hora em que o registro foi inserido no sistema.
        /// </summary>
        public DateTime DataInsercao { get; protected set; }

        /// <summary>
        /// Indica se o registro foi excluído logicamente (Soft Delete).
        /// Registros com IsDeleted = true devem ser ignorados nas queries.
        /// </summary>
        public bool IsDeleted { get; protected set; }

        /// <summary>
        /// Data e hora da exclusão lógica. Null enquanto não excluído.
        /// </summary>
        public DateTime? DeletedAt { get; protected set; }

        protected EntityBase()
        {
            DataInsercao = DateTime.UtcNow;
            IsDeleted    = false;
            DeletedAt    = null;
        }

        /// <summary>
        /// Marca o registro como excluído logicamente (Soft Delete).
        /// Nunca remove o dado do banco — apenas o torna inativo e invisível nas consultas.
        /// </summary>
        public virtual void SoftDelete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Restaura um registro que havia sido excluído logicamente.
        /// </summary>
        public virtual void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
        }

        /// <summary>
        /// Comparação por identidade (ID), não por referência de memória.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is not EntityBase other) return false;
            if (ReferenceEquals(this, other)) return true;
            if (Id == 0 || other.Id == 0) return false;
            return Id == other.Id && GetType() == other.GetType();
        }

        public override int GetHashCode() =>
            (GetType().GetHashCode() * 31) + Id.GetHashCode();
    }
}
