using SmartPark.Domain.Primitives;
using SmartPark.Domain.ValueObjects;
using SmartPark.Domain.Enums;
using System;

namespace SmartPark.Domain.Entities
{
    public sealed class Ticket : AggregateRoot
    {
        public Guid CompanyId { get; private set; }
        public Guid ParkingId { get; private set; }
        public Guid? VehicleId { get; private set; }
        public Guid? OperatorId { get; private set; }
        public string TicketNumber { get; private set; } = null!;
        public DateTime IssuedAt { get; private set; }
        public DateTime? ExpiresAt { get; private set; }
        public TicketStatus Status { get; private set; }
        private Ticket() : base(Guid.Empty) { }
        public Ticket(
            Guid id,
            Guid companyId,
            Guid parkingId,
            string ticketNumber,
            Guid? vehicleId = null,
            Guid? operatorId = null,
            DateTime? expiresAt = null) : base(id)
        {
            if (companyId == Guid.Empty)
                throw new ArgumentException("O identificador da empresa (CompanyId) é obrigatório.", nameof(companyId));
            if (parkingId == Guid.Empty)
                throw new ArgumentException("O identificador do estacionamento (ParkingId) é obrigatório.", nameof(parkingId));
            if (string.IsNullOrWhiteSpace(ticketNumber))
                throw new ArgumentException("O número do ticket é obrigatório.", nameof(ticketNumber));
            CompanyId = companyId;
            ParkingId = parkingId;
            TicketNumber = ticketNumber.Trim().ToUpperInvariant();
            VehicleId = vehicleId;
            OperatorId = operatorId;
            IssuedAt = DateTime.Now;
            ExpiresAt = expiresAt;
            Status = TicketStatus.Open;
        }
        public void MarkAsUsed()
        {
            if (Status != TicketStatus.Open)
                throw new InvalidOperationException("Apenas tickets abertos podem ser marcados como utilizados.");
            UpdatedAt = DateTime.Now;
            Status = TicketStatus.Used;
        }
        public void MarkAsLost()
        {
            if (Status != TicketStatus.Open)
                throw new InvalidOperationException("Apenas tickets abertos podem ser declarados como perdidos.");
            UpdatedAt = DateTime.Now;
            Status = TicketStatus.Lost;
        }
        public void Cancel()
        {
            if (Status == TicketStatus.Used)
                throw new InvalidOperationException("Não é possível cancelar um ticket que já foi utilizado.");
            UpdatedAt = DateTime.Now;
            Status = TicketStatus.Cancelled;
        }
    }
}