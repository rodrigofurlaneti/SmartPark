using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Comercial
{
    public class VagaCortesiaVigencia : EntityBase
    {
        public DateTime DataInicio { get; private set; }
        public DateTime DataFim { get; private set; }
        public int VagaCortesia_Id { get; private set; }

        public bool IsValida() => DateTime.Now >= DataInicio && DateTime.Now <= DataFim;
    }
}
