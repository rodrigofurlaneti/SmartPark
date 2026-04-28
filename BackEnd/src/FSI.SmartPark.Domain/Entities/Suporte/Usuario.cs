namespace FSI.SmartPark.Domain.Entities.Suporte
{
    public class Usuario : EntityBase
    {
        public Usuario() { }

        public Usuario(string login, string senha, int empresaId, int? unidadeId = null)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentException("Login é obrigatório.", nameof(login));
            if (string.IsNullOrWhiteSpace(senha))
                throw new ArgumentException("Senha é obrigatória.", nameof(senha));
            if (empresaId <= 0)
                throw new ArgumentException("EmpresaId é obrigatório.", nameof(empresaId));

            Login      = login;
            Senha      = senha;
            Ativo      = true;
            Empresa_Id = empresaId;
            Unidade_Id = unidadeId;
        }

        public string  Login        { get; private set; } = string.Empty;
        public string  Senha        { get; private set; } = string.Empty;
        public bool    Ativo        { get; private set; }
        public int?    Funcionario_Id { get; private set; }
        public int?    Unidade_Id   { get; private set; }

        /// <summary>Tenant — isola o usuário ao contexto da empresa contratante.</summary>
        public int Empresa_Id { get; private set; }

        public void Bloquear() => Ativo = false;
    }
}
