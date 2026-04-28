namespace FSI.SmartPark.Domain.Entities.Suporte
{
    public class Estoque : EntityBase
    {
        public Estoque() { }

        public Estoque(string nome, int unidadeId, int empresaId, bool principal = false)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome do estoque e obrigatorio.", nameof(nome));
            if (empresaId <= 0)
                throw new ArgumentException("EmpresaId e obrigatorio.", nameof(empresaId));

            Nome             = nome;
            Unidade_Id       = unidadeId;
            Empresa_Id       = empresaId;
            EstoquePrincipal = principal;
        }

        public string? Nome             { get; private set; }
        public bool    EstoquePrincipal { get; private set; }
        public int?    Unidade_Id       { get; private set; }

        /// <summary>Tenant - isola o estoque ao contexto da empresa contratante.</summary>
        public int Empresa_Id { get; private set; }
    }
}
