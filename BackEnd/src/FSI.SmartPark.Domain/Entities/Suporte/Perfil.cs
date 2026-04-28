using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Suporte
{
    public class Perfil : EntityBase
    {
        public string Nome { get; private set; }
        public Perfil(string nome) => Nome = nome;
    }
}
