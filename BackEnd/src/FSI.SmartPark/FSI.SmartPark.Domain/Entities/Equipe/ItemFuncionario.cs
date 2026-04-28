using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Equipe
{
    public class ItemFuncionario : EntityBase
    {
        public int Funcionario_Id { get; private set; }
        public ItemFuncionario(int funcionarioId) => Funcionario_Id = funcionarioId;
    }
}
