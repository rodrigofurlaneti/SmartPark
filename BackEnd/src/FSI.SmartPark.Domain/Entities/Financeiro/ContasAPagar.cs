namespace FSI.SmartPark.Domain.Entities.Financeiro
{
    public class ContasAPagar : EntityBase
    {
        public ContasAPagar() { }

        public ContasAPagar(string doc, DateTime vencimento, decimal valor, int empresaId)
        {
            if (valor <= 0)
                throw new ArgumentException("Valor deve ser positivo.", nameof(valor));
            if (empresaId <= 0)
                throw new ArgumentException("EmpresaId é obrigatório.", nameof(empresaId));

            NumeroDocumento = doc;
            DataVencimento  = vencimento;
            ValorTotal      = valor;
            StatusConta     = 1; // Aberto
            Empresa_Id      = empresaId;
        }

        public string   NumeroDocumento  { get; private set; } = string.Empty;
        public DateTime DataVencimento   { get; private set; }
        public decimal  ValorTotal       { get; private set; }
        public int      StatusConta      { get; private set; }
        public string?  CodigoDeBarras   { get; private set; }
        public int?     Fornecedor_Id    { get; private set; }
        public int?     Unidade_Id       { get; private set; }

        /// <summary>Tenant — isola o lançamento ao contexto da empresa contratante.</summary>
        public int Empresa_Id { get; private set; }

        public void Pagar() => StatusConta = 2;
    }
}
