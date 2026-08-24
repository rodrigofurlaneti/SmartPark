using SmartPark.Domain.Primitives;
using SmartPark.Domain.ValueObjects;
using SmartPark.Domain.Enums;
using System;

namespace SmartPark.Domain.Entities
{
    public sealed class CashRegister : AggregateRoot
    {
        public Guid CompanyId { get; private set; }
        public Guid ParkingId { get; private set; }
        public Guid OpenedBy { get; private set; }
        public DateTime OpenedAt { get; private set; }
        public Money OpeningAmount { get; private set; } = Money.Zero;
        public Guid? ClosedBy { get; private set; }
        public DateTime? ClosedAt { get; private set; }
        public Money? ExpectedAmount { get; private set; }
        public Money? ClosingAmount { get; private set; }
        public Money? DifferenceAmount { get; private set; }
        public CashRegisterStatus Status { get; private set; }
        private CashRegister() : base(Guid.Empty) { }
        public CashRegister(
            Guid id,
            Guid companyId,
            Guid parkingId,
            Guid openedBy,
            Money openingAmount) : base(id)
        {
            if (openingAmount.Amount < 0)
                throw new ArgumentException("O valor de abertura do caixa não pode ser negativo.", nameof(openingAmount));
            CompanyId = companyId;
            ParkingId = parkingId;
            OpenedBy = openedBy;
            OpenedAt = DateTime.UtcNow;
            OpeningAmount = openingAmount;
            Status = CashRegisterStatus.Open;
        }

        public void Close(Guid closedBy, Money expectedAmount, Money closingAmount)
        {
            if (Status != CashRegisterStatus.Open)
                throw new InvalidOperationException("Apenas caixas abertos podem ser fechados.");
            ClosedBy = closedBy;
            ClosedAt = DateTime.UtcNow;
            ExpectedAmount = expectedAmount;
            ClosingAmount = closingAmount;
            var difference = closingAmount.Amount - expectedAmount.Amount;
            DifferenceAmount = new Money(Math.Abs(difference)); 
            Status = CashRegisterStatus.Closed;
            UpdatedAt = DateTime.Now;
        }
        public void Cancel()
        {
            UpdatedAt = DateTime.Now;
            if (Status == CashRegisterStatus.Closed)
                throw new InvalidOperationException("Não é possível cancelar um caixa que já foi fechado.");
            Status = CashRegisterStatus.Cancelled;
        }
    }
}