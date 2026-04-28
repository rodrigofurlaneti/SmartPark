using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Suporte
{
    public class Material : EntityBase
    {
        public string Nome { get; private set; }
        public int EstoqueMinimo { get; private set; }
        public int EstoqueMaximo { get; private set; }
        public int? TipoMaterial_Id { get; private set; }

        public Material(string nome, int min)
        {
            Nome = nome;
            EstoqueMinimo = min;
        }
    }
}
