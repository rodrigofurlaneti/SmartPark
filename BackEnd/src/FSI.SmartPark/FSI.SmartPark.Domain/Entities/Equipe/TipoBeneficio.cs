namespace FSI.SmartPark.Domain.Entities.Equipe
{
    public class TipoBeneficio : EntityBase
    {
        public string Descricao { get; private set; }
        public TipoBeneficio(string descricao) => Descricao = descricao;
    }
}
