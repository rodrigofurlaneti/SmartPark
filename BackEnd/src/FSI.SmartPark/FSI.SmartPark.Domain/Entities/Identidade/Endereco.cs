namespace FSI.SmartPark.Domain.Entities.Identidade
{
    public class Endereco : EntityBase
    {
        public string Cep { get; private set; }
        public string Logradouro { get; private set; }
        public string Numero { get; private set; }
        public string Bairro { get; private set; }
        public int? Cidade_Id { get; private set; }
    }
}
