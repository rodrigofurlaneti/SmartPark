using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Financeiro
{
    public class ContasAPagar : EntityBase
    {
        public ContasAPagar() { }
        public ContasAPagar(string doc, DateTime vencimento, decimal valor)
        {
            if (valor <= 0) throw new Exception("Valor deve ser positivo");
            NumeroDocumento = doc;
            DataVencimento = vencimento;
            ValorTotal = valor;
            StatusConta = 1; // Aberto
        }

        public string NumeroDocumento { get; private set; }
        public DateTime DataVencimento { get; private set; }
        public decimal ValorTotal { get; private set; }
        public int StatusConta { get; private set; }
        public string? CodigoDeBarras { get; private set; }
        public int? Fornecedor_Id { get; private set; }
        public int? Unidade_Id { get; private set; }

        public void Pagar() => StatusConta = 2;
    }
}
