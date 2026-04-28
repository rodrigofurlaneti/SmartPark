using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Comercial
{
    public class ClienteCondomino : EntityBase
    {
        public int NumeroVagas { get; private set; }
        public bool Frota { get; private set; }
        public int Cliente_Id { get; private set; }
        public int Unidade_Id { get; private set; }

        public ClienteCondomino(int vagas, bool frota, int clienteId, int unidadeId)
        {
            NumeroVagas = vagas;
            Frota = frota;
            Cliente_Id = clienteId;
            Unidade_Id = unidadeId;
        }
    }
}
