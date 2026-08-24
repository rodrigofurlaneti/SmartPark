using SmartPark.Domain.Primitives;
using SmartPark.Domain.ValueObjects;
using SmartPark.Domain.Enums;
using System;

namespace SmartPark.Domain.Entities
{
    public sealed class Invoice : AggregateRoot
    {
        public Guid CompanyId { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid? PostpaidAccountId { get; private set; }
        public string InvoiceNumber { get; private set; } = null!;
        public DateTime IssueDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public Money Subtotal { get; private set; } = Money.Zero;
        public Money DiscountAmount { get; private set; } = Money.Zero;
        public Money TotalAmount { get; private set; } = Money.Zero;
        public InvoiceStatus Status { get; private set; }
        private Invoice() : base(Guid.Empty) { }
        public Invoice(
            Guid id,
            Guid companyId,
            Guid customerId,
            string invoiceNumber,
            DateTime issueDate,
            DateTime dueDate,
            Money subtotal,
            Money? discountAmount = null,
            Guid? postpaidAccountId = null) : base(id)
        {
            if (companyId == Guid.Empty)
                throw new ArgumentException("O identificador da empresa (CompanyId) é obrigatório.", nameof(companyId));
            if (customerId == Guid.Empty)
                throw new ArgumentException("O identificador do cliente (CustomerId) é obrigatório.", nameof(customerId));
            if (string.IsNullOrWhiteSpace(invoiceNumber))
                throw new ArgumentException("O número da fatura é obrigatório.", nameof(invoiceNumber));
            if (dueDate < issueDate)
                throw new ArgumentException("A data de vencimento não pode ser anterior à data de emissão.", nameof(dueDate));
            if (subtotal is null || subtotal.Amount < 0)
                throw new ArgumentException("O subtotal da fatura não pode ser negativo.", nameof(subtotal));
            CompanyId = companyId;
            CustomerId = customerId;
            PostpaidAccountId = postpaidAccountId;
            InvoiceNumber = invoiceNumber.Trim().ToUpperInvariant();
            IssueDate = issueDate.Date;
            DueDate = dueDate.Date;
            Subtotal = subtotal;
            DiscountAmount = discountAmount ?? Money.Zero;
            var total = Math.Max(0, Subtotal.Amount - DiscountAmount.Amount);
            TotalAmount = new Money(total);
            Status = InvoiceStatus.Draft;
        }
        public void Issue()
        {
            if (Status != InvoiceStatus.Draft)
                throw new InvalidOperationException("Apenas faturas em rascunho (Draft) podem ser emitidas.");
            Status = InvoiceStatus.Issued;
            UpdatedAt = DateTime.Now;
        }
        public void MarkAsOpen()
        {
            if (Status != InvoiceStatus.Issued)
                throw new InvalidOperationException("A fatura precisa ser emitida antes de ficar em aberto (Open).");
            Status = InvoiceStatus.Open;
            UpdatedAt = DateTime.Now;
        }
        public void MarkAsPaid()
        {
            if (Status == InvoiceStatus.Cancelled || Status == InvoiceStatus.Draft)
                throw new InvalidOperationException("Faturas canceladas ou em rascunho não podem ser pagas.");
            Status = InvoiceStatus.Paid;
            UpdatedAt = DateTime.Now;
        }
        public void MarkAsOverdue()
        {
            if (Status != InvoiceStatus.Open && Status != InvoiceStatus.Issued)
                throw new InvalidOperationException("Apenas faturas pendentes podem ser marcadas como vencidas.");
            Status = InvoiceStatus.Overdue;
            UpdatedAt = DateTime.Now;
        }
        public void Cancel()
        {
            if (Status == InvoiceStatus.Paid)
                throw new InvalidOperationException("Faturas já pagas não podem ser canceladas.");
            Status = InvoiceStatus.Cancelled;
            UpdatedAt = DateTime.Now;
        }
    }
}