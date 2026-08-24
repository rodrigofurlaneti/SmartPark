using SmartPark.Domain.Primitives;
using SmartPark.Domain.Enums;
using System;

namespace SmartPark.Domain.Entities
{
    public sealed class User : AggregateRoot
    {
        public Guid CompanyId { get; private set; }
        public string Name { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string PasswordHash { get; private set; } = null!;
        public UserStatus Status { get; private set; }
        public DateTime? LastLoginAt { get; private set; }
        private User() : base(Guid.Empty) { }
        public User(Guid id, Guid companyId, string name, string email, string passwordHash) : base(id)
        {
            if (companyId == Guid.Empty)
                throw new ArgumentException("O identificador da empresa (CompanyId) é obrigatório.", nameof(companyId));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome do usuário é obrigatório.", nameof(name));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("O e-mail do usuário é obrigatório.", nameof(email));
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("O hash de senha é obrigatório.", nameof(passwordHash));
            CompanyId = companyId;
            Name = name.Trim();
            Email = email.Trim().ToLowerInvariant();
            PasswordHash = passwordHash;
            Status = UserStatus.Active; 
        }
        public void UpdateDetails(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome do usuário é obrigatório.", nameof(name));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("O e-mail do usuário é obrigatório.", nameof(email));
            Name = name.Trim();
            Email = email.Trim().ToLowerInvariant();
            UpdatedAt = DateTime.Now;
        }
        public void ChangePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new ArgumentException("O novo hash de senha não pode ser vazio.", nameof(newPasswordHash));
            PasswordHash = newPasswordHash;
            UpdatedAt = DateTime.Now;
        }
        public void RecordLogin()
        {
            if (Status != UserStatus.Active)
                throw new InvalidOperationException("Usuários inativos ou bloqueados não podem registrar login.");
            LastLoginAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
        public void Block()
        {
            Status = UserStatus.Blocked;
            UpdatedAt = DateTime.Now;
        }
        public void Activate()
        {
            Status = UserStatus.Active;
            UpdatedAt = DateTime.Now;
        }
    }
}