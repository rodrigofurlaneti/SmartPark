using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Comercial
{
    public class VagaCortesia : EntityBase
    {
        public int Cliente_Id { get; private set; }
        public VagaCortesia(int clienteId) => Cliente_Id = clienteId;
    }
}
