using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Financeiro
{
    public class LancamentoCobranca : EntityBase
    {
        public DateTime DataVencimento { get; private set; }
        public decimal ValorContrato { get; private set; }
        public decimal ValorMulta { get; private set; }
        public decimal ValorJuros { get; private set; }
        public int StatusLancamentoCobranca { get; private set; }
        public int? Cliente_Id { get; private set; }

        public void AplicarTaxas(decimal multa, decimal juros)
        {
            ValorMulta = multa;
            ValorJuros = juros;
        }

        public decimal CalcularTotal() => ValorContrato + ValorMulta + ValorJuros;
    }
}
