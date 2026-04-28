namespace FSI.SmartPark.Domain.Entities.Identidade
{
    public class Filial : EntityBase
    {
        public int Empresa_Id { get; private set; }
        public int PessoaJuridica_Id { get; private set; }
        public int? TipoFilial_Id { get; private set; }
    }
}
