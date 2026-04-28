using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.SmartPark.Domain.Entities.Equipe
{
    public class BeneficioFuncionarioDetalhe : EntityBase
    {
        public decimal Valor { get; private set; }
        public int BeneficioFuncionario_Id { get; private set; }
        public int TipoBeneficio_Id { get; private set; }

        public BeneficioFuncionarioDetalhe(int beneficioId, int tipoId, decimal valor)
        {
            if (valor < 0) throw new Exception("Valor do benefício não pode ser negativo");
            BeneficioFuncionario_Id = beneficioId;
            TipoBeneficio_Id = tipoId;
            Valor = valor;
        }
    }
}
