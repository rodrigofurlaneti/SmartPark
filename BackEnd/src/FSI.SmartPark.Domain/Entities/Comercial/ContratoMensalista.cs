using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Comercial
{
    public class ContratoMensalista : EntityBase
    {
        public int NumeroContrato { get; private set; }
        public DateTime DataVencimento { get; private set; }
        public decimal Valor { get; private set; }
        public bool Ativo { get; private set; }
        public int NumeroVagas { get; private set; }
        public string HorarioInicio { get; private set; }
        public string HorarioFim { get; private set; }
        public int Cliente_Id { get; private set; }
        public int Unidade_Id { get; private set; }

        public ContratoMensalista(int clienteId, int unidadeId, decimal valor)
        {
            Cliente_Id = clienteId;
            Unidade_Id = unidadeId;
            Valor = valor;
            Ativo = true;
            DataVencimento = DateTime.Now.AddMonths(1);
        }

        public void BloquearContrato() => Ativo = false;
        public void RenovarContrato() => DataVencimento = DataVencimento.AddMonths(1);
    }
}
