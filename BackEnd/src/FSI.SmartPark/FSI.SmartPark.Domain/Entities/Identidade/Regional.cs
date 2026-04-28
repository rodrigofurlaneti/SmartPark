using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Identidade
{
    public class Regional : EntityBase
    {
        public string Descricao { get; private set; }
        public Regional(string descricao) => Descricao = descricao;
    }
}
