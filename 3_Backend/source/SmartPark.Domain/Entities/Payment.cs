using SmartPark.Domain.Primitives;
using SmartPark.Domain.ValueObjects;
using SmartPark.Domain.Enums;
using System;

namespace SmartPark.Domain.Entities
{
    public sealed class Payment : AggregateRoot
    {
        public Guid CompanyId { get; private set; }
        public Guid PaymentMethodId { get; private set; }
        public Money Amount { get; private set; } = Money.Zero;
        public DateTime PaidAt { get; private set; }
        public PaymentStatus Status { get; private set; }
        public string? ExternalReference { get; private set; }
        public string? Metadata { get; private set; } 
        public Guid? CreatedBy { get; private set; }
        private Payment() : base(Guid.Empty) { }
        public Payment(
            Guid id,
            Guid companyId,
            Guid paymentMethodId,
            Money amount,
            string? externalReference = null,
            string? metadata = null,
            Guid? createdBy = null) : base(id)
        {
            if (companyId == Guid.Empty)
                throw new ArgumentException("O identificador da empresa (CompanyId) é obrigatório.", nameof(companyId));
            if (paymentMethodId == Guid.Empty)
                throw new ArgumentException("O identificador do método de pagamento (PaymentMethodId) é obrigatório.", nameof(paymentMethodId));
            if (amount is null || amount.Amount <= 0)
                throw new ArgumentException("O valor do pagamento deve ser maior que zero.", nameof(amount));
            CompanyId = companyId;
            PaymentMethodId = paymentMethodId;
            Amount = amount;
            PaidAt = DateTime.Now;
            Status = PaymentStatus.Pending;
            ExternalReference = externalReference;
            Metadata = metadata;
            CreatedBy = createdBy;
        }

        public void Complete(string? externalReference = null)
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Apenas pagamentos pendentes podem ser concluídos.");
            if (!string.IsNullOrWhiteSpace(externalReference))
                ExternalReference = externalReference;
            PaidAt = DateTime.Now;
            Status = PaymentStatus.Completed;
            UpdatedAt = DateTime.Now;
        }

        public void Cancel()
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Apenas pagamentos pendentes podem ser cancelados.");
            Status = PaymentStatus.Cancelled;
            UpdatedAt = DateTime.Now;
        }

        public void Refund()
        {
            if (Status != PaymentStatus.Completed)
                throw new InvalidOperationException("Apenas pagamentos concluídos podem ser reembolsados.");
            Status = PaymentStatus.Refunded;
            UpdatedAt = DateTime.Now;
        }
    }
}