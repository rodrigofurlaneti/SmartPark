namespace FSI.SmartPark.Domain.Entities.Operacional
{
    public class EstruturaUnidade : EntityBase
    {
        public string Descricao { get; private set; }
        public int Unidade_Id { get; private set; }
        public int? EstruturaGaragem_Id { get; private set; }
    }
}
