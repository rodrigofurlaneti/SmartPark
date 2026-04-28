namespace FSI.SmartPark.Domain.Entities.Operacional
{
    public class Faturamento : EntityBase
    {
        // Construtor vazio para o Dapper
        public Faturamento() { }

        public Faturamento(int numFechamento, int numTerminal, int unidadeId, int usuarioId, int empresaId)
        {
            if (empresaId <= 0)
                throw new ArgumentException("EmpresaId é obrigatório.", nameof(empresaId));

            NumFechamento = numFechamento;
            NumTerminal   = numTerminal;
            Unidade_Id    = unidadeId;
            Usuario_Id    = usuarioId;
            Empresa_Id    = empresaId;
            DataAbertura  = DateTime.UtcNow;
        }

        public int      NumFechamento    { get; private set; }
        public DateTime DataAbertura     { get; private set; }
        public DateTime DataFechamento   { get; private set; }
        public decimal  ValorTotal       { get; private set; }
        public decimal  ValorDinheiro    { get; private set; }
        public decimal  ValorCartaoDebito  { get; private set; }
        public decimal  ValorCartaoCredito { get; private set; }
        public int      Unidade_Id       { get; private set; }
        public int?     IdSoftpark       { get; private set; }
        public string?  NomeUnidade      { get; private set; }
        public int      NumTerminal      { get; private set; }
        public string?  TicketInicial    { get; private set; }
        public string?  TicketFinal      { get; private set; }
        public string?  PatioAtual       { get; private set; }
        public decimal? ValorRotativo    { get; private set; }
        public decimal? ValorRecebimentoMensalidade { get; private set; }
        public decimal? ValorSemParar    { get; private set; }
        public decimal? ValorSeloDesconto { get; private set; }
        public decimal? SaldoInicial     { get; private set; }
        public decimal? ValorSangria     { get; private set; }
        public int?     Usuario_Id       { get; private set; }

        /// <summary>Tenant — isola o faturamento ao contexto da empresa contratante.</summary>
        public int Empresa_Id { get; private set; }

        public void FecharTurno(
            decimal valorTotal, decimal dinheiro,
            decimal cartaoDebito, decimal cartaoCredito,
            decimal? valorRotativo = null, decimal? valorMensalidade = null,
            decimal? valorSemParar = null, decimal? valorSeloDesconto = null,
            string? ticketFinal = null)
        {
            DataFechamento              = DateTime.UtcNow;
            ValorTotal                  = valorTotal;
            ValorDinheiro               = dinheiro;
            ValorCartaoDebito           = cartaoDebito;
            ValorCartaoCredito          = cartaoCredito;
            ValorRotativo               = valorRotativo;
            ValorRecebimentoMensalidade = valorMensalidade;
            ValorSemParar               = valorSemParar;
            ValorSeloDesconto           = valorSeloDesconto;
            TicketFinal                 = ticketFinal;
        }

        public void DefinirSaldoInicial(decimal saldo) => SaldoInicial = saldo;
        public void RegistrarSangria(decimal valor)    => ValorSangria  = (ValorSangria ?? 0) + valor;
        public void DefinirTicketInicial(string ticket) => TicketInicial = ticket;
    }
}
