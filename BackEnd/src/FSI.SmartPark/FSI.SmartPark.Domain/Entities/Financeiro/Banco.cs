using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Financeiro
{
    public class Banco : EntityBase
    {
        public string CodigoBanco { get; private set; }
        public string Descricao { get; private set; }
    }
}
