namespace FSI.SmartPark.Domain.Entities.Suporte
{
    public class Menu : EntityBase
    {
        public string Descricao { get; private set; }
        public string Url { get; private set; }
        public int Posicao { get; private set; }
        public int? MenuPai_Id { get; private set; }
    }
}
