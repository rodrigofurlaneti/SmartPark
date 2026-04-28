using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Suporte
{
    public class Estoque : EntityBase
    {
        public string Nome { get; private set; }
        public bool EstoquePrincipal { get; private set; }
        public int? Unidade_Id { get; private set; }
    }
}
