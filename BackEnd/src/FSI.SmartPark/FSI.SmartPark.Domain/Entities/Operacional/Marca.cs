namespace FSI.SmartPark.Domain.Entities.Operacional
{
    public class Marca : EntityBase
    {
        public string Nome { get; private set; }

        protected Marca() { } // Dapper

        public Marca(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Nome da marca é obrigatório");
            Nome = nome;
        }
    }
}