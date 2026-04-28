namespace FSI.SmartPark.Domain.Entities
{
    /// <summary>
    /// Classe base para todas as entidades do sistema SmartPark.
    /// Aplica encapsulamento para garantir a integridade da identidade.
    /// </summary>
    public abstract class EntityBase
    {
        // O Id é definido pelo banco (Auto-increment), por isso o set é protected.
        // Isso impede que a camada de aplicação altere o ID manualmente.
        public int Id { get; protected set; }

        // Data de registro no sistema. 
        // No Object Calisthenics, iniciamos com um valor padrão para evitar nulos.
        public DateTime DataInsercao { get; protected set; }

        protected EntityBase()
        {
            DataInsercao = DateTime.Now;
        }

        /// <summary>
        /// Sobrescrita do Equals para garantir que a comparação entre entidades 
        /// seja feita pela identidade (ID) e não pela referência de memória.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is not EntityBase other) return false;
            if (ReferenceEquals(this, other)) return true;
            if (Id == 0 || other.Id == 0) return false;

            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 31) + Id.GetHashCode();
        }
    }
}