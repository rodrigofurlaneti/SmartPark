using SmartPark.Domain.Primitives;
using SmartPark.Domain.Enums;
using System;

namespace SmartPark.Domain.Entities
{
    public sealed class Parking : AggregateRoot
    {
        public Guid CompanyId { get; private set; }
        public Guid EstablishmentId { get; private set; }
        public string Code { get; private set; } = null!;
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; }
        public int? Capacity { get; private set; }
        public TimeSpan? OpeningTime { get; private set; }
        public TimeSpan? ClosingTime { get; private set; }
        public ParkingStatus Status { get; private set; }
        private Parking() : base(Guid.Empty) { }
        public Parking(
            Guid id,
            Guid companyId,
            Guid establishmentId,
            string code,
            string name,
            int? capacity = null,
            TimeSpan? openingTime = null,
            TimeSpan? closingTime = null,
            string? description = null) : base(id)
        {
            if (companyId == Guid.Empty)
                throw new ArgumentException("O identificador da empresa (CompanyId) é obrigatório.", nameof(companyId));
            if (establishmentId == Guid.Empty)
                throw new ArgumentException("O identificador do estabelecimento (EstablishmentId) é obrigatório.", nameof(establishmentId));
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("O código do estacionamento é obrigatório.", nameof(code));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome do estacionamento é obrigatório.", nameof(name));
            if (capacity.HasValue && capacity.Value < 0)
                throw new ArgumentException("A capacidade do estacionamento não pode ser negativa.", nameof(capacity));
            CompanyId = companyId;
            EstablishmentId = establishmentId;
            Code = code.Trim().ToUpperInvariant();
            Name = name.Trim();
            Capacity = capacity;
            OpeningTime = openingTime;
            ClosingTime = closingTime;
            Description = description;
            Status = ParkingStatus.Active;
        }
        public void UpdateDetails(string name, string? description, int? capacity, TimeSpan? openingTime, TimeSpan? closingTime)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome do estacionamento é obrigatório.", nameof(name));
            Name = name.Trim();
            Description = description;
            Capacity = capacity;
            OpeningTime = openingTime;
            ClosingTime = closingTime;
            UpdatedAt = DateTime.Now;
        }
        public void PutUnderMaintenance()
        {
            Status = ParkingStatus.Maintenance;
            UpdatedAt = DateTime.Now;
        }
        public void Deactivate()
        {
            Status = ParkingStatus.Inactive;
            UpdatedAt = DateTime.Now;
        }
        public void Activate()
        {
            Status = ParkingStatus.Active;
            UpdatedAt = DateTime.Now;
        }
    }
}