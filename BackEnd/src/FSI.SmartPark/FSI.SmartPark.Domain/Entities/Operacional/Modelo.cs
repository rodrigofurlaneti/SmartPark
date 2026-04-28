using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Operacional
{
    public class Modelo : EntityBase
    {
        public string Descricao { get; private set; }
        public int Marca_Id { get; private set; }

        protected Modelo() { } // Dapper

        public Modelo(string descricao, int marcaId)
        {
            if (string.IsNullOrWhiteSpace(descricao)) throw new ArgumentException("Descrição do modelo é obrigatória");
            Descricao = descricao;
            Marca_Id = marcaId;
        }
    }
}