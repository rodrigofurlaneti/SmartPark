namespace FSI.SmartPark.Domain.Entities.Operacional
{
    public class Movimentacao : EntityBase
    {
        // ✅ Construtor vazio para o Dapper
        public Movimentacao() { }

        public Movimentacao(string placa, int unidadeId)
        {
            if (string.IsNullOrWhiteSpace(placa))
                throw new ArgumentException("Placa é obrigatória.");
            Placa = placa.ToUpper().Replace("-", "");
            Unidade_Id = unidadeId;
            DataEntrada = DateTime.Now;
            DataAbertura = DateTime.Now;
            Ticket = Guid.NewGuid().ToString()[..8].ToUpper();
            // ✅ DataFechamento e DataSaida NÃO são preenchidos aqui
        }

        public string? Ticket { get; private set; }
        public string? Placa { get; private set; }
        public DateTime DataEntrada { get; private set; }
        public DateTime? DataSaida { get; private set; }  // ✅ nullable
        public DateTime? DataAbertura { get; private set; }  // ✅ nullable
        public DateTime? DataFechamento { get; private set; }  // ✅ nullable — só preenche na saída
        public decimal ValorCobrado { get; private set; }
        public decimal ValorDesconto { get; private set; }
        public bool VagaIsenta { get; private set; }
        public int Unidade_Id { get; private set; }
        public int NumFechamento { get; private set; }
        public int NumTerminal { get; private set; }
        public int? IdSoftpark { get; private set; }
        public int? Usuario_Id { get; private set; }
        public int? Cliente_Id { get; private set; }
        public string? DescontoUtilizado { get; private set; }
        public string? TipoCliente { get; private set; }
        public string? NumeroContrato { get; private set; }
        public string? Cpf { get; private set; }
        public string? Rps { get; private set; }
        public string? FormaPagamento { get; private set; }

        public void RegistrarSaida(decimal valor, string? formaPagamento = null)
        {
            if (DataSaida.HasValue)
                throw new InvalidOperationException("Veículo já registrou saída.");
            DataSaida = DateTime.Now;
            DataFechamento = DateTime.Now;
            ValorCobrado = valor;
            FormaPagamento = formaPagamento;
        }

        public void AplicarDesconto(string tipoDesconto, decimal valorDesconto)
        {
            DescontoUtilizado = tipoDesconto;
            ValorDesconto = valorDesconto;
        }

        public void VincularCliente(int clienteId, string? numeroContrato = null)
        {
            Cliente_Id = clienteId;
            NumeroContrato = numeroContrato;
            TipoCliente = numeroContrato is not null ? "Mensalista" : "Avulso";
        }

        public void InformarCpfParaNF(string cpf) => Cpf = cpf;
        public void MarcarVagaIsenta() => VagaIsenta = true;
    }
}