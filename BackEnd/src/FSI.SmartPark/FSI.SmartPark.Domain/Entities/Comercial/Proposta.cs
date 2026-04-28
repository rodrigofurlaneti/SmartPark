namespace FSI.SmartPark.Domain.Entities.Comercial
{
    public class Proposta : EntityBase
    {
        public int Cliente_Id { get; private set; }
        public decimal ValorTotal { get; private set; }
        public DateTime DataValidade { get; private set; }
        public string Status { get; private set; } // Sugestão: Usar Enum mais tarde

        public Proposta(int clienteId, decimal valor, DateTime validade)
        {
            if (valor <= 0) throw new ArgumentException("Valor da proposta deve ser positivo");
            Cliente_Id = clienteId;
            ValorTotal = valor;
            DataValidade = validade;
            Status = "Pendente";
        }

        public void Aprovar() => Status = "Aprovado";
    }
}