using FSI.SmartPark.Domain.ValueObjects;

namespace FSI.SmartPark.Domain.Entities.Comercial
{
    public class Cliente : EntityBase
    {
        // Construtor protegido para o Dapper
        protected Cliente() { }

        public Cliente(string nome, Cpf cpf, bool isMensalista, int empresaId)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome é obrigatório.", nameof(nome));
            if (empresaId <= 0)
                throw new ArgumentException("EmpresaId é obrigatório.", nameof(empresaId));

            Nome            = nome;
            DocumentoNumero = cpf.Numero;
            IsMensalista    = isMensalista;
            Ativo           = true;
            Empresa_Id      = empresaId;
        }

        public Cliente(string nome, Cnpj cnpj, bool isMensalista, int empresaId)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome é obrigatório.", nameof(nome));
            if (empresaId <= 0)
                throw new ArgumentException("EmpresaId é obrigatório.", nameof(empresaId));

            Nome            = nome;
            DocumentoNumero = cnpj.Numero;
            IsMensalista    = isMensalista;
            Ativo           = true;
            Empresa_Id      = empresaId;
        }

        public string  Nome            { get; private set; } = string.Empty;
        public string  DocumentoNumero { get; private set; } = string.Empty;
        public bool    IsMensalista    { get; private set; }
        public bool    Ativo           { get; private set; }

        /// <summary>Tenant — isola o cliente ao contexto da empresa contratante.</summary>
        public int Empresa_Id { get; private set; }

        public void AlterarStatus(bool novoStatus) => Ativo = novoStatus;
    }
}
