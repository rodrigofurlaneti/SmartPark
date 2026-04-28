namespace FSI.SmartPark.Domain.Entities.Operacional
{
    public class Unidade : EntityBase
    {
        // ✅ Construtor vazio para o Dapper
        public Unidade() { }

        public Unidade(string nome, int vagas, int diaVencimento, int empresaId)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome da unidade é obrigatório.");
            if (vagas < 0)
                throw new ArgumentException("Capacidade de vagas não pode ser negativa.");
            if (diaVencimento < 1 || diaVencimento > 31)
                throw new ArgumentException("Dia de vencimento deve ser entre 1 e 31.");
            Nome = nome;
            NumeroVaga = vagas;
            DiaVencimento = diaVencimento;
            Empresa_Id = empresaId;
            Ativa = true;
        }

        public string Codigo { get; private set; }
        public string Nome { get; private set; }
        public int NumeroVaga { get; private set; }
        public bool Ativa { get; private set; }
        public string HorarioInicial { get; private set; }
        public string HorarioFinal { get; private set; }
        public int DiaVencimento { get; private set; }
        public string? CNPJ { get; private set; }
        public string? CCM { get; private set; }
        public int? Empresa_Id { get; private set; }
        public int? Funcionario_Id { get; private set; }
        public int? Endereco_Id { get; private set; }

        public void AtualizarCapacidade(int novasVagas)
        {
            if (novasVagas < 0) throw new ArgumentException("Capacidade não pode ser negativa.");
            NumeroVaga = novasVagas;
        }

        public void DefinirResponsavel(int funcionarioId) => Funcionario_Id = funcionarioId;
        public void VincularEndereco(int enderecoId) => Endereco_Id = enderecoId;
        public void DefinirCNPJ(string cnpj) => CNPJ = cnpj;
        public void Inativar() => Ativa = false;
        public void Ativar() => Ativa = true;
    }
}
