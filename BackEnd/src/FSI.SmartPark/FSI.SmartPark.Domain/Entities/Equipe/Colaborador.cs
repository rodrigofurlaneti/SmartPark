using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Equipe
{
    public class Colaborador : EntityBase
    {
        public int Funcionario_Id { get; private set; }
        public int? PeriodoHorario_Id { get; private set; }
    }
}
