using FSI.SmartPark.Domain.Enums;

namespace FSI.SmartPark.Domain.Entities.Equipe
{
    public class Equipe : EntityBase
    {
        public string Nome { get; private set; }
        public bool Ativo { get; private set; }
        public int Unidade_Id { get; private set; }
        public int? Encarregado_Id { get; private set; }

        // [A18] Campos adicionados conforme modelagem MySQL
        public DateTime? DataFim { get; private set; }         // Data de encerramento da equipe
        public int? TipoEquipe_Id { get; private set; }        // Categoria da equipe (vigilância, manobrista, etc.)
        public int? Funcionamento_Id { get; private set; }     // Escala de funcionamento vinculada
        public TipoHorario TipoHorario { get; private set; }   // Diurno, Noturno, Misto
        public int? Supervisor_Id { get; private set; }        // Funcionário supervisor (diferente do encarregado)

        public Equipe(string nome, int unidadeId, TipoHorario tipoHorario)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome da equipe é obrigatório.");

            Nome = nome;
            Unidade_Id = unidadeId;
            TipoHorario = tipoHorario;
            Ativo = true;
        }

        public void DefinirEncarregado(int funcionarioId) => Encarregado_Id = funcionarioId;
        public void DefinirSupervisor(int funcionarioId)  => Supervisor_Id = funcionarioId;
        public void VincularTipo(int tipoEquipeId)        => TipoEquipe_Id = tipoEquipeId;
        public void VincularFuncionamento(int id)         => Funcionamento_Id = id;

        public void Encerrar(DateTime dataFim)
        {
            if (dataFim < DataInsercao)
                throw new ArgumentException("Data de encerramento não pode ser anterior à criação.");
            DataFim = dataFim;
            Ativo = false;
        }

        public bool EstaEncerrada() => DataFim.HasValue && DataFim <= DateTime.Now;
    }
}
