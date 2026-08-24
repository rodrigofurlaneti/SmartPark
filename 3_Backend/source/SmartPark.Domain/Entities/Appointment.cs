using SmartPark.Domain.Primitives;
using SmartPark.Domain.Enums;
using System;

namespace SmartPark.Domain.Entities
{
    public sealed class Appointment : AggregateRoot
    {
        public Guid CompanyId { get; private set; }
        public Guid? ParkingId { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid VehicleId { get; private set; }
        public Guid ServiceId { get; private set; }
        public DateTime ScheduledAt { get; private set; }
        public AppointmentStatus Status { get; private set; }
        public string? Notes { get; private set; }
        private Appointment() : base(Guid.Empty) { }
        public Appointment(
            Guid id,
            Guid companyId,
            Guid? parkingId,
            Guid customerId,
            Guid vehicleId,
            Guid serviceId,
            DateTime scheduledAt,
            string? notes = null) : base(id)
        {
            if (scheduledAt < DateTime.UtcNow)
                throw new ArgumentException("A data do agendamento não pode ser no passado.", nameof(scheduledAt));
            CompanyId = companyId;
            ParkingId = parkingId;
            CustomerId = customerId;
            VehicleId = vehicleId;
            ServiceId = serviceId;
            ScheduledAt = scheduledAt;
            Status = AppointmentStatus.Scheduled;
            Notes = notes;
        }

        public void Confirm()
        {
            UpdatedAt = DateTime.Now;
            if (Status != AppointmentStatus.Scheduled)
                throw new InvalidOperationException("Apenas agendamentos com status 'Scheduled' podem ser confirmados.");
            Status = AppointmentStatus.Confirmed;
        }

        public void Complete()
        {
            UpdatedAt = DateTime.Now;
            Status = AppointmentStatus.Completed;
        }
        public void Cancel()
        {
            UpdatedAt = DateTime.Now;
            Status = AppointmentStatus.Cancelled;
        }
    }
}