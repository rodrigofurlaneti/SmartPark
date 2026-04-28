namespace FSI.SmartPark.Domain.Entities.Comercial
{
    public class Convenio : EntityBase
    {
        public string NomeParceiro { get; private set; }
        public decimal PercentualDesconto { get; private set; }
        public bool Ativo { get; private set; }

        public Convenio(string nome, decimal desconto)
        {
            NomeParceiro = nome;
            PercentualDesconto = (desconto >= 0 && desconto <= 100) ? desconto : throw new Exception("Desconto inválido");
            Ativo = true;
        }
    }
}