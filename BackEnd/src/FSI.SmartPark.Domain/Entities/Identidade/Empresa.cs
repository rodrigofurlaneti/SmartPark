namespace FSI.SmartPark.Domain.Entities.Identidade
{
    /// <summary>
    /// Representa um tenant/empresa que utiliza o SmartPark.
    /// Todas as entidades do domínio devem referenciar Empresa_Id para isolamento multi-tenant.
    /// </summary>
    public class Empresa : EntityBase
    {
        // ✅ Construtor vazio necessário para o Dapper (mapeamento por reflexão)
        public Empresa() { }

        /// <summary>
        /// Cria uma nova empresa validando os campos obrigatórios.
        /// </summary>
        public Empresa(string nomeFantasia, string cnpj, string? razaoSocial = null, string? email = null, string? telefone = null)
        {
            if (string.IsNullOrWhiteSpace(nomeFantasia))
                throw new ArgumentException("Nome fantasia é obrigatório.", nameof(nomeFantasia));

            if (string.IsNullOrWhiteSpace(cnpj))
                throw new ArgumentException("CNPJ é obrigatório.", nameof(cnpj));

            var cnpjLimpo = cnpj.Replace(".", "").Replace("/", "").Replace("-", "").Trim();
            if (cnpjLimpo.Length != 14)
                throw new ArgumentException("CNPJ deve ter 14 dígitos.", nameof(cnpj));

            NomeFantasia = nomeFantasia.Trim();
            Cnpj         = cnpjLimpo;
            RazaoSocial  = razaoSocial?.Trim();
            Email        = email?.Trim().ToLowerInvariant();
            Telefone     = telefone?.Trim();
            Ativa        = true;
        }

        // ──────────────────────────── Propriedades ────────────────────────────

        /// <summary>Nome comercial / fantasia da empresa exibido no sistema.</summary>
        public string NomeFantasia { get; private set; } = string.Empty;

        /// <summary>Razão social oficial da empresa (opcional no cadastro inicial).</summary>
        public string? RazaoSocial { get; private set; }

        /// <summary>CNPJ armazenado apenas com dígitos (14 chars), sem pontuação.</summary>
        public string Cnpj { get; private set; } = string.Empty;

        /// <summary>E-mail de contato principal da empresa.</summary>
        public string? Email { get; private set; }

        /// <summary>Telefone de contato principal da empresa.</summary>
        public string? Telefone { get; private set; }

        /// <summary>Indica se a empresa está ativa e pode operar no sistema.</summary>
        public bool Ativa { get; private set; }

        // ──────────────────────────── Comportamentos ──────────────────────────

        /// <summary>Atualiza os dados cadastrais da empresa.</summary>
        public void AtualizarDados(string nomeFantasia, string? razaoSocial, string? email, string? telefone)
        {
            if (string.IsNullOrWhiteSpace(nomeFantasia))
                throw new ArgumentException("Nome fantasia é obrigatório.", nameof(nomeFantasia));

            NomeFantasia = nomeFantasia.Trim();
            RazaoSocial  = razaoSocial?.Trim();
            Email        = email?.Trim().ToLowerInvariant();
            Telefone     = telefone?.Trim();
        }

        /// <summary>Ativa a empresa, permitindo seu uso no sistema.</summary>
        public void Ativar() => Ativa = true;

        /// <summary>Inativa a empresa, bloqueando novos registros sob este tenant.</summary>
        public void Inativar() => Ativa = false;
    }
}
