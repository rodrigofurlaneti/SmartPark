using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Financeiro
{
    public class ContaCorrenteCliente : EntityBase
    {
        public int Cliente_Id { get; private set; }
        public ContaCorrenteCliente(int clienteId) => Cliente_Id = clienteId;
    }
}
