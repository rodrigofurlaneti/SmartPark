using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Comercial
{
    public class TipoSelo : EntityBase
    {
        public string Nome { get; private set; }
        public decimal Valor { get; private set; }
        public bool ComValidade { get; private set; }

        public TipoSelo(string nome, decimal valor, bool comValidade)
        {
            Nome = nome;
            Valor = valor > 0 ? valor : throw new Exception("Valor do selo deve ser positivo");
            ComValidade = comValidade;
        }
    }
}
