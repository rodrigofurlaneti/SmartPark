using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Operacional
{
    public class PeriodoHorario : EntityBase
    {
        public string Inicio { get; private set; }
        public string Fim { get; private set; }
        public string Periodo { get; private set; } // Ex: "Diurno", "Noturno"
    }
}
