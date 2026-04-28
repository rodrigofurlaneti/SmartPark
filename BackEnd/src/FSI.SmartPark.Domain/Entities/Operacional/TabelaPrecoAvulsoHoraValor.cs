namespace FSI.SmartPark.Domain.Entities.Operacional
{
    public class TabelaPrecoAvulsoHoraValor : EntityBase
    {
        public int Hora { get; private set; }
        public decimal Valor { get; private set; }
        public int TabelaPrecoAvulso_Id { get; private set; }
    }
}
