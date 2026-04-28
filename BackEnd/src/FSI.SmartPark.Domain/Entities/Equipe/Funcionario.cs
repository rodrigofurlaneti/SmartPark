using FSI.SmartPark.Domain.Enums;

namespace FSI.SmartPark.Domain.Entities.Equipe
{
    public class Funcionario : EntityBase
    {
        public Funcionario() { }

        public Funcionario(int pessoaId, decimal salario, TipoEscalaFuncionario escala, int empresaId)
        {
            if (salario < 0)
                throw new ArgumentException("Salário não pode ser negativo.", nameof(salario));
            if (empresaId <= 0)
                throw new ArgumentException("EmpresaId é obrigatório.", nameof(empresaId));

            Pessoa_Id    = pessoaId;
            Salario      = salario;
            TipoEscala   = escala;
            Status       = StatusFuncionario.Ativo;
            DataAdmissao = DateTime.UtcNow;
            Empresa_Id   = empresaId;
        }

        public string?              Codigo       { get; private set; }
        public decimal              Salario      { get; private set; }
        public StatusFuncionario    Status       { get; private set; }
        public TipoEscalaFuncionario TipoEscala { get; private set; }
        public DateTime?            DataAdmissao { get; private set; }
        public int                  Pessoa_Id    { get; private set; }
        public int?                 Cargo_Id     { get; private set; }
        public int?                 Unidade_Id   { get; private set; }
        public int?                 Supervisor_Id { get; private set; }

        /// <summary>Tenant — isola o funcionário ao contexto da empresa contratante.</summary>
        public int Empresa_Id { get; private set; }

        public void AlterarSalario(decimal novoValor)
        {
            if (novoValor < 0) throw new ArgumentException("Salário não pode ser negativo.", nameof(novoValor));
            Salario = novoValor;
        }

        public void Desligar()      => Status = StatusFuncionario.Desligado;
        public void Afastar()       => Status = StatusFuncionario.Afastado;
        public void IniciarFerias() => Status = StatusFuncionario.Ferias;
        public void Reativar()      => Status = StatusFuncionario.Ativo;
    }
}
