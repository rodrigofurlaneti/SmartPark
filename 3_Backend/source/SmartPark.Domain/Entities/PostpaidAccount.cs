using SmartPark.Domain.Primitives;
using SmartPark.Domain.ValueObjects;
using SmartPark.Domain.Enums;
using System;

namespace SmartPark.Domain.Entities
{
    public sealed class PostpaidAccount : AggregateRoot
    {
        public Guid CompanyId { get; private set; }
        public Guid CustomerId { get; private set; }
        public string AccountNumber { get; private set; } = null!;
        public Money? CreditLimit { get; private set; }
        public int? BillingDay { get; private set; }
        public int? DueDay { get; private set; }
        public PostpaidAccountStatus Status { get; private set; }
        private PostpaidAccount() : base(Guid.Empty) { }
        public PostpaidAccount(
            Guid id,
            Guid companyId,
            Guid customerId,
            string accountNumber,
            Money? creditLimit = null,
            int? billingDay = null,
            int? dueDay = null) : base(id)
        {
            if (companyId == Guid.Empty)
                throw new ArgumentException("O identificador da empresa (CompanyId) é obrigatório.", nameof(companyId));
            if (customerId == Guid.Empty)
                throw new ArgumentException("O identificador do cliente (CustomerId) é obrigatório.", nameof(customerId));
            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentException("O número da conta pós-pago é obrigatório.", nameof(accountNumber));
            if (billingDay.HasValue && (billingDay.Value < 1 || billingDay.Value > 31))
                throw new ArgumentException("O dia de faturamento deve estar entre 1 e 31.", nameof(billingDay));
            if (dueDay.HasValue && (dueDay.Value < 1 || dueDay.Value > 31))
                throw new ArgumentException("O dia de vencimento deve estar entre 1 e 31.", nameof(dueDay));
            CompanyId = companyId;
            CustomerId = customerId;
            AccountNumber = accountNumber.Trim();
            CreditLimit = creditLimit;
            BillingDay = billingDay;
            DueDay = dueDay;
            Status = PostpaidAccountStatus.Active;
        }
        public void UpdateCreditLimit(Money? newLimit)
        {
            CreditLimit = newLimit;
            UpdatedAt = DateTime.Now;
        }
        public void UpdateBillingSchedule(int? billingDay, int? dueDay)
        {
            if (billingDay.HasValue && (billingDay.Value < 1 || billingDay.Value > 31))
                throw new ArgumentException("O dia de faturamento deve estar entre 1 e 31.", nameof(billingDay));
            if (dueDay.HasValue && (dueDay.Value < 1 || dueDay.Value > 31))
                throw new ArgumentException("O dia de vencimento deve estar entre 1 e 31.", nameof(dueDay));
            BillingDay = billingDay;
            DueDay = dueDay;
            UpdatedAt = DateTime.Now;
        }
        public void Block()
        {
            if (Status == PostpaidAccountStatus.Closed)
                throw new InvalidOperationException("Não é possível bloquear uma conta que já está fechada.");
            Status = PostpaidAccountStatus.Blocked;
            UpdatedAt = DateTime.Now;
        }
        public void Activate()
        {
            if (Status == PostpaidAccountStatus.Closed)
                throw new InvalidOperationException("Não é possível reativar uma conta que já foi encerrada.");
            Status = PostpaidAccountStatus.Active;
            UpdatedAt = DateTime.Now;
        }
        public void Close()
        {
            Status = PostpaidAccountStatus.Closed;
            UpdatedAt = DateTime.Now;
        }
    }
}