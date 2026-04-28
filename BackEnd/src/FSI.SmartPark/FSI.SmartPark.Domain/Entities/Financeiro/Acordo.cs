namespace FSI.SmartPark.Domain.Entities.Financeiro
{
    public class Acordo : EntityBase
    {
        public string Descricao { get; private set; }
        public decimal ValorTotal { get; private set; }
        public int? Cliente_Id { get; private set; }
        public int? Unidade_Id { get; private set; }

        public Acordo(string descricao, decimal valor, int clienteId)
        {
            if (valor <= 0) throw new Exception("Valor do acordo deve ser positivo");
            Descricao = descricao;
            ValorTotal = valor;
            Cliente_Id = clienteId;
        }
    }


}