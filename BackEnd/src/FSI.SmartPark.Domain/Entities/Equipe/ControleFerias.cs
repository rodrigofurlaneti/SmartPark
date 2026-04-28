using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Equipe
{
    public class ControleFerias : EntityBase
    {
        public DateTime DataInicio { get; private set; }
        public DateTime DataFim { get; private set; }
        public int Funcionario_Id { get; private set; }

        public ControleFerias(int funcionarioId, DateTime inicio, DateTime fim)
        {
            if (fim <= inicio) throw new Exception("Data fim inválida");
            Funcionario_Id = funcionarioId;
            DataInicio = inicio;
            DataFim = fim;
        }
    }
}
