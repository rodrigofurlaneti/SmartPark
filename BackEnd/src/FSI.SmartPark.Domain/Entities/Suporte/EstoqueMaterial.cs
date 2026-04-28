using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Suporte
{
    public class EstoqueMaterial : EntityBase
    {
        public int Quantidade { get; private set; }
        public int Estoque_Id { get; private set; }
        public int Material_Id { get; private set; }

        public void AdicionarQuantidade(int qtd) => Quantidade += qtd;
        public void RemoverQuantidade(int qtd)
        {
            if (Quantidade < qtd) throw new Exception("Estoque insuficiente");
            Quantidade -= qtd;
        }
    }
}
