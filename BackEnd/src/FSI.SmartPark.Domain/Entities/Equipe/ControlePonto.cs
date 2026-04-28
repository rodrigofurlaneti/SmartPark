namespace FSI.SmartPark.Domain.Entities.Equipe
{
    public class ControlePonto : EntityBase
    {
        public DateTime DataRegistro { get; private set; }
        public int TipoRegistro { get; private set; } // 1=Entrada, 2=Saída
        public int Funcionario_Id { get; private set; }
        public int? Unidade_Id { get; private set; }

        public ControlePonto(int funcionarioId, int tipo)
        {
            Funcionario_Id = funcionarioId;
            TipoRegistro = tipo;
            DataRegistro = DateTime.Now;
        }
    }
}
