using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Suporte
{
    public class Audit : EntityBase
    {
        public string Entidade { get; private set; }
        public string Atributo { get; private set; }
        public string ValorAntigo { get; private set; }
        public string ValorNovo { get; private set; }
        public int UsuarioId { get; private set; }
    }
}
