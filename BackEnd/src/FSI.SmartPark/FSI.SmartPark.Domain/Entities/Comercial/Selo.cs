using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Comercial
{
    public class Selo : EntityBase
    {
        public int Sequencial { get; private set; }
        public DateTime? Validade { get; private set; }
        public int EmissaoSelo_Id { get; private set; }

        public Selo(int sequencial, int emissaoId, DateTime? validade = null)
        {
            Sequencial = sequencial;
            EmissaoSelo_Id = emissaoId;
            Validade = validade;
        }

        public bool IsExpirado() => Validade.HasValue && Validade < DateTime.Now;
    }
}
