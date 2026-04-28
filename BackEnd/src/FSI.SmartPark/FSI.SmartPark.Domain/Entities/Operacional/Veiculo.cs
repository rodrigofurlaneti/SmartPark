using FSI.SmartPark.Domain.Enums;
using FSI.SmartPark.Domain.ValueObjects;

namespace FSI.SmartPark.Domain.Entities.Operacional
{
    public class Veiculo : EntityBase
    {
        public Placa Placa { get; private set; }
        public string? Cor { get; private set; }
        public int? Ano { get; private set; }
        public TipoVeiculo TipoVeiculo { get; private set; }
        public int? Modelo_Id { get; private set; }

        protected Veiculo() { }

        public Veiculo(string placa, TipoVeiculo tipoVeiculo, int? modeloId = null)
        {
            Placa = new Placa(placa);
            TipoVeiculo = tipoVeiculo;
            Modelo_Id = modeloId;
        }

        public void AtualizarInformacoesAdicionais(string cor, int ano)
        {
            if (ano > DateTime.Now.Year + 1)
                throw new ArgumentException("O ano do veículo é inválido.");
            Cor = cor;
            Ano = ano;
        }

        public void VincularModelo(int modeloId)
        {
            if (modeloId <= 0) throw new ArgumentException("Id do modelo inválido.");
            Modelo_Id = modeloId;
        }
    }
}