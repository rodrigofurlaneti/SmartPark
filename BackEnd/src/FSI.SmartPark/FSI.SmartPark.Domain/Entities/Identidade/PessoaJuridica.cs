namespace FSI.SmartPark.Domain.Entities.Identidade
{
    using FSI.SmartPark.Domain.ValueObjects;

    public class PessoaJuridica : EntityBase
    {
        public int Pessoa_Id { get; private set; }
        public Cnpj CNPJ { get; private set; }
        public string IE { get; private set; } // Inscrição Estadual
        public string IM { get; private set; } // Inscrição Municipal

        public PessoaJuridica(int pessoaId, Cnpj cnpj)
        {
            Pessoa_Id = pessoaId;
            CNPJ = cnpj;
        }
    }
}