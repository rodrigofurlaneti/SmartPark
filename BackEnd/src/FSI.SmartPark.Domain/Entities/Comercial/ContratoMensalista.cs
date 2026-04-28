namespace FSI.SmartPark.Domain.Entities.Comercial
{
    public class ContratoMensalista : EntityBase
    {
        public ContratoMensalista() { }

        public ContratoMensalista(int clienteId, int unidadeId, decimal valor, int empresaId)
        {
            if (valor <= 0)
                throw new ArgumentException("Valor do contrato deve ser positivo.", nameof(valor));
            if (empresaId <= 0)
                throw new ArgumentException("EmpresaId é obrigatório.", nameof(empresaId));

            Cliente_Id     = clienteId;
            Unidade_Id     = unidadeId;
            Valor          = valor;
            Ativo          = true;
            DataVencimento = DateTime.UtcNow.AddMonths(1);
            Empresa_Id     = empresaId;
        }

        public int      NumeroContrato  { get; private set; }
        public DateTime DataVencimento  { get; private set; }
        public decimal  Valor           { get; private set; }
        public bool     Ativo           { get; private set; }
        public int      NumeroVagas     { get; private set; }
        public string?  HorarioInicio   { get; private set; }
        public string?  HorarioFim      { get; private set; }
        public int      Cliente_Id      { get; private set; }
        public int      Unidade_Id      { get; private set; }

        /// <summary>Tenant — isola o contrato ao contexto da empresa contratante.</summary>
        public int Empresa_Id { get; private set; }

        public void BloquearContrato() => Ativo = false;
        public void RenovarContrato()  => DataVencimento = DataVencimento.AddMonths(1);
    }
}
