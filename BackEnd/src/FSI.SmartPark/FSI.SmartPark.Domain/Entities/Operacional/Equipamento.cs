namespace FSI.SmartPark.Domain.Entities.Operacional
{
    public class Equipamento : EntityBase
    {
        public string Descricao { get; private set; }
        public bool Ativo { get; private set; }

        public void Inativar() => Ativo = false;
    }
}
