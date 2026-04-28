namespace FSI.SmartPark.Domain.Entities.Equipe
{
    public class FuncionarioIntervaloNoturno : EntityBase
    {
        public DateTime DataInicial { get; private set; }
        public DateTime DataFinal { get; private set; }
        public int Funcionario_Id { get; private set; }
    }
}
