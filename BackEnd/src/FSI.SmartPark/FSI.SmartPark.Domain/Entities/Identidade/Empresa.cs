using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Identidade
{
    public class Empresa : EntityBase
    {
        public int PessoaJuridica_Id { get; private set; }
        public int? Grupo_Id { get; private set; }

        public Empresa(int pjId) => PessoaJuridica_Id = pjId;
    }
}
