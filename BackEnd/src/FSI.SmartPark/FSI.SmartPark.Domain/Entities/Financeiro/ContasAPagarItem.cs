using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Financeiro
{
    public class ContasAPagarItem : EntityBase
    {
        public decimal Valor { get; private set; }
        public int ContasAPagar_Id { get; private set; }
        public int? ContaContabil_Id { get; private set; }
    }
}
