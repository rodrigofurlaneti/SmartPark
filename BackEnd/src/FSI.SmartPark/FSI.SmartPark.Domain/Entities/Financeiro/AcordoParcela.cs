namespace FSI.SmartPark.Domain.Entities.Financeiro
{
    public class AcordoParcela : EntityBase
    {
        public int Numero { get; private set; }
        public DateTime DataVencimento { get; private set; }
        public decimal Valor { get; private set; }
        public bool Pago { get; private set; }
        public int Acordo_Id { get; private set; }

        public void RegistrarPagamento() => Pago = true;
    }
}
