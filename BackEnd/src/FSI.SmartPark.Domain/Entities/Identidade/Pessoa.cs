namespace FSI.SmartPark.Domain.Entities.Identidade
{
    public class Pessoa : EntityBase
    {
        public string Nome { get; private set; }
        public string Sexo { get; private set; }
        public DateTime? DataNascimento { get; private set; }
        public bool Ativo { get; private set; }

        public Pessoa(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Nome é obrigatório");
            Nome = nome;
            Ativo = true;
        }

        public void AlterarNome(string novoNome) => Nome = novoNome;
        public void Inativar() => Ativo = false;
    }
}