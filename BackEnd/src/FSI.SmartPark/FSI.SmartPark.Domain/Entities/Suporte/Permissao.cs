using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Suporte
{
    public class Permissao : EntityBase
    {
        public string Nome { get; private set; }
        public string Regra { get; private set; } // Ex: "PROPOSTA_APROVAR"
    }
}
