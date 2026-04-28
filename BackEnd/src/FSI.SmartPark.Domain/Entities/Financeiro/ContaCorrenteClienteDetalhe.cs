namespace FSI.SmartPark.Domain.Entities.Financeiro
{
    public class ContaCorrenteClienteDetalhe : EntityBase
    {
        public DateTime DataCompetencia { get; private set; }
        public decimal Valor { get; private set; }
        public int TipoOperacao { get; private set; } // 1=Crédito, 2=Débito
        public int ContaCorrenteCliente_Id { get; private set; }
    }
}
