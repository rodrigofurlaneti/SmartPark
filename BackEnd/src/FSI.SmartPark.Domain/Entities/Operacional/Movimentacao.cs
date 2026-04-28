namespace FSI.SmartPark.Domain.Entities.Operacional
{
    public class Movimentacao : EntityBase
    {
        // Construtor vazio para o Dapper
        public Movimentacao() { }

        public Movimentacao(string placa, int unidadeId, int empresaId)
        {
            if (string.IsNullOrWhiteSpace(placa))
                throw new ArgumentException("Placa é obrigatória.", nameof(placa));
            if (empresaId <= 0)
                throw new ArgumentException("EmpresaId é obrigatório.", nameof(empresaId));

            Placa        = placa.ToUpperInvariant().Replace("-", "");
            Unidade_Id   = unidadeId;
            Empresa_Id   = empresaId;
            DataEntrada  = DateTime.UtcNow;
            DataAbertura = DateTime.UtcNow;
            Ticket       = Guid.NewGuid().ToString()[..8].ToUpper();
        }

        public string?   Ticket          { get; private set; }
        public string?   Placa           { get; private set; }
        public DateTime  DataEntrada     { get; private set; }
        public DateTime? DataSaida       { get; private set; }
        public DateTime? DataAbertura    { get; private set; }
        public DateTime? DataFechamento  { get; private set; }
        public decimal   ValorCobrado    { get; private set; }
        public decimal   ValorDesconto   { get; private set; }
        public bool      VagaIsenta      { get; private set; }
        public int       Unidade_Id      { get; private set; }
        public int       NumFechamento   { get; private set; }
        public int       NumTerminal     { get; private set; }
        public int?      IdSoftpark      { get; private set; }
        public int?      Usuario_Id      { get; private set; }
        public int?      Cliente_Id      { get; private set; }
        public string?   DescontoUtilizado { get; private set; }
        public string?   TipoCliente     { get; private set; }
        public string?   NumeroContrato  { get; private set; }
        public string?   Cpf             { get; private set; }
        public string?   Rps             { get; private set; }
        public string?   FormaPagamento  { get; private set; }

        /// <summary>Tenant — isola a movimentação ao contexto da empresa contratante.</summary>
        public int Empresa_Id { get; private set; }

        public void RegistrarSaida(decimal valor, string? formaPagamento = null)
        {
            if (DataSaida.HasValue)
                throw new InvalidOperationException("Veículo já registrou saída.");
            DataSaida      = DateTime.UtcNow;
            DataFechamento = DateTime.UtcNow;
            ValorCobrado   = valor;
            FormaPagamento = formaPagamento;
        }

        public void AplicarDesconto(string tipoDesconto, decimal valorDesconto)
        {
            DescontoUtilizado = tipoDesconto;
            ValorDesconto     = valorDesconto;
        }

        public void VincularCliente(int clienteId, string? numeroContrato = null)
        {
            Cliente_Id      = clienteId;
            NumeroContrato  = numeroContrato;
            TipoCliente     = numeroContrato is not null ? "Mensalista" : "Avulso";
        }

        public void InformarCpfParaNF(string cpf) => Cpf = cpf;
        public void MarcarVagaIsenta() => VagaIsenta = true;
    }
}
