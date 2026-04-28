using FSI.SmartPark.Domain.Enums;

namespace FSI.SmartPark.Domain.Entities.Operacional
{
    public class TabelaPrecoAvulso : EntityBase
    {
        public string Nome { get; private set; }
        public int TempoToleranciaDesistencia { get; private set; }
        public int TempoToleranciaPagamento { get; private set; }
        public decimal ValorHoraAdicional { get; private set; }
        public bool Padrao { get; private set; }

        // [A16] Campos adicionados conforme modelagem MySQL
        public int Numero { get; private set; }                    // Número sequencial da tabela
        public StatusSolicitacao Status { get; private set; }      // Pendente, Aprovado, Reprovado
        public string? HoraInicioVigencia { get; private set; }    // Hora inicial de aplicação (ex: "08:00")
        public string? HoraFimVigencia { get; private set; }       // Hora final de aplicação (ex: "22:00")
        public int QuantidadeHoraAdicional { get; private set; }   // Qtd de horas para acionar cobrança adicional
        public bool HoraAdicional { get; private set; }            // Se cobra hora adicional após tolerância
        public string? DescricaoHoraValor { get; private set; }    // Texto descritivo da faixa de preço
        public int? Usuario_Id { get; private set; }               // Usuário que criou/aprovou

        public TabelaPrecoAvulso(string nome, decimal valorAdicional, int usuarioId)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome da tabela de preço é obrigatório.");
            if (valorAdicional < 0)
                throw new ArgumentException("Valor de hora adicional não pode ser negativo.");

            Nome = nome;
            ValorHoraAdicional = valorAdicional;
            Usuario_Id = usuarioId;
            Status = StatusSolicitacao.Pendente;
        }

        public void Aprovar()
        {
            if (Status != StatusSolicitacao.Pendente)
                throw new InvalidOperationException("Apenas tabelas pendentes podem ser aprovadas.");
            Status = StatusSolicitacao.Aprovado;
        }

        public void Reprovar() => Status = StatusSolicitacao.Reprovado;

        public void DefinirVigencia(string horaInicio, string horaFim)
        {
            HoraInicioVigencia = horaInicio;
            HoraFimVigencia = horaFim;
        }

        public void DefinirComoPadrao() => Padrao = true;

        public void HabilitarHoraAdicional(int quantidade)
        {
            HoraAdicional = true;
            QuantidadeHoraAdicional = quantidade;
        }
    }
}
