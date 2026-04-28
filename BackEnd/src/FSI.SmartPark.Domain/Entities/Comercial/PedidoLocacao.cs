using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Comercial
{
    public class PedidoLocacao : EntityBase
    {
        public decimal ValorTotal { get; private set; }
        public bool PossuiFiador { get; private set; }
        public string NomeFiador { get; private set; }
        public DateTime DataVigenciaInicio { get; private set; }
        public int? Cliente_Id { get; private set; }
        public int? Unidade_Id { get; private set; }

        public PedidoLocacao(decimal valor, int clienteId)
        {
            ValorTotal = valor;
            Cliente_Id = clienteId;
        }
    }
}
