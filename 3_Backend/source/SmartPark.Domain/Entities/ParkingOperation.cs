using SmartPark.Domain.Primitives;
using SmartPark.Domain.ValueObjects;
using SmartPark.Domain.Enums;
using System;

namespace SmartPark.Domain.Entities
{
    public sealed class ParkingOperation : AggregateRoot
    {
        public Guid CompanyId { get; private set; }
        public Guid ParkingId { get; private set; }
        public Guid? TicketId { get; private set; }
        public Guid VehicleId { get; private set; }
        public Guid? CustomerId { get; private set; }
        public Guid? PricingTableId { get; private set; }
        public OperationType OperationType { get; private set; }
        public OperationStatus Status { get; private set; }
        public DateTime EntryAt { get; private set; }
        public DateTime? ExitAt { get; private set; }
        public Money CalculatedAmount { get; private set; } = Money.Zero;
        public Money DiscountAmount { get; private set; } = Money.Zero;
        public Money FinalAmount { get; private set; } = Money.Zero;
        public string? PricingSnapshot { get; private set; }
        public Guid? EntryOperatorId { get; private set; }
        public Guid? ExitOperatorId { get; private set; }
        private ParkingOperation() : base(Guid.Empty) { }
        public ParkingOperation(
            Guid id,
            Guid companyId,
            Guid parkingId,
            Guid vehicleId,
            OperationType operationType,
            Guid? entryOperatorId = null,
            Guid? ticketId = null,
            Guid? customerId = null) : base(id)
        {
            if (companyId == Guid.Empty)
                throw new ArgumentException("O identificador da empresa (CompanyId) é obrigatório.", nameof(companyId));
            if (parkingId == Guid.Empty)
                throw new ArgumentException("O identificador do estacionamento (ParkingId) é obrigatório.", nameof(parkingId));
            if (vehicleId == Guid.Empty)
                throw new ArgumentException("O identificador do veículo (VehicleId) é obrigatório.", nameof(vehicleId));
            CompanyId = companyId;
            ParkingId = parkingId;
            VehicleId = vehicleId;
            OperationType = operationType;
            EntryOperatorId = entryOperatorId;
            TicketId = ticketId;
            CustomerId = customerId;
            EntryAt = DateTime.UtcNow;
            Status = OperationStatus.InParking;
        }
        public void RegisterExit(
            DateTime exitAt,
            Money calculatedAmount,
            Money discount,
            string? snapshot,
            Guid? exitOperatorId = null,
            Guid? pricingTableId = null)
        {
            if (Status != OperationStatus.InParking)
                throw new InvalidOperationException("Apenas operações com veículos dentro do estacionamento podem registrar saída.");
            if (exitAt < EntryAt)
                throw new ArgumentException("A data/hora de saída não pode ser anterior à data/hora de entrada.", nameof(exitAt));
            ExitAt = exitAt;
            CalculatedAmount = calculatedAmount ?? throw new ArgumentNullException(nameof(calculatedAmount));
            DiscountAmount = discount ?? Money.Zero;
            var finalValue = Math.Max(0, calculatedAmount.Amount - DiscountAmount.Amount);
            FinalAmount = new Money(finalValue);
            PricingSnapshot = snapshot;
            ExitOperatorId = exitOperatorId;
            PricingTableId = pricingTableId;
            Status = OperationStatus.WaitingPayment;
            UpdatedAt = DateTime.Now;
        }
        public void MarkAsPaid()
        {
            if (Status != OperationStatus.WaitingPayment)
                throw new InvalidOperationException("A operação precisa estar aguardando pagamento para ser marcada como paga.");
            Status = OperationStatus.Paid;
            UpdatedAt = DateTime.Now;
        }
        public void Complete()
        {
            if (Status != OperationStatus.Paid && OperationType != OperationType.Postpaid && OperationType != OperationType.Monthly)
                throw new InvalidOperationException("A operação precisa estar paga ou ser do tipo faturada/mensal para ser concluída.");
            Status = OperationStatus.Completed;
            UpdatedAt = DateTime.Now;
        }
        public void Cancel()
        {
            if (Status == OperationStatus.Completed || Status == OperationStatus.Paid)
                throw new InvalidOperationException("Não é possível cancelar uma operação que já foi paga ou concluída.");
            Status = OperationStatus.Cancelled;
            UpdatedAt = DateTime.Now;
        }
    }
}