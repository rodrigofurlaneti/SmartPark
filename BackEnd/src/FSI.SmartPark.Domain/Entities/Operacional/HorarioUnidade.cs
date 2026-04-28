using FSI.SmartPark.Domain.Enums;

namespace FSI.SmartPark.Domain.Entities.Operacional
{
    public class HorarioUnidade : EntityBase
    {
        public string Nome { get; private set; }
        public DateTime DataValidade { get; private set; }
        public int Unidade_Id { get; private set; }
        public bool Feriados { get; private set; }

        // [A10] Campos obrigatórios adicionados conforme modelagem MySQL
        public bool Fixo { get; private set; }                  // Horário fixo ou variável por período
        public TipoHorario TipoHorario { get; private set; }    // Diurno, Noturno, Misto, etc.
        public StatusHorario Status { get; private set; }       // Ativo, Inativo, Pendente

        public HorarioUnidade(string nome, int unidadeId, TipoHorario tipo, DateTime dataValidade)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome do horário é obrigatório.");

            Nome = nome;
            Unidade_Id = unidadeId;
            TipoHorario = tipo;
            DataValidade = dataValidade;
            Status = StatusHorario.Ativo;
        }

        public bool EstaVigente() => DataValidade >= DateTime.Now && Status == StatusHorario.Ativo;

        public void Inativar()                           => Status = StatusHorario.Inativo;
        public void MarcarComoFixo()                     => Fixo = true;
        public void PermitirFeriados()                   => Feriados = true;
        public void RenovarValidade(DateTime novaData)
        {
            if (novaData <= DateTime.Now)
                throw new ArgumentException("Nova validade deve ser futura.");
            DataValidade = novaData;
        }
    }
}
