using FSI.SmartPark.Domain.ValueObjects;

namespace FSI.SmartPark.Domain.Entities.Comercial
{
    public class Cliente : EntityBase
    {
        public string Nome { get; private set; }
        public string DocumentoNumero { get; private set; }
        public bool IsMensalista { get; private set; }
        public bool Ativo { get; private set; }

        // Construtor protegido para o Dapper
        protected Cliente() { }

        // Construtor usando Cpf (Exemplo para Mensalista Pessoa Física)
        public Cliente(string nome, Cpf cpf, bool isMensalista)
        {
            if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Nome é obrigatório");

            Nome = nome;
            DocumentoNumero = cpf.Numero; // Extrai o valor já validado pelo VO
            IsMensalista = isMensalista;
            Ativo = true;
        }

        // Sobrecarga de construtor para Pessoa Jurídica (Empresas/Convênios)
        public Cliente(string nome, Cnpj cnpj, bool isMensalista)
        {
            if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Nome é obrigatório");

            Nome = nome;
            DocumentoNumero = cnpj.Numero; // Extrai o valor já validado pelo VO
            IsMensalista = isMensalista;
            Ativo = true;
        }

        public void AlterarStatus(bool novoStatus) => Ativo = novoStatus;
    }
}