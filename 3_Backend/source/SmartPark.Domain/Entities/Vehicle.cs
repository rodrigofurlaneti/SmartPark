using SmartPark.Domain.Primitives;
using SmartPark.Domain.ValueObjects;
using SmartPark.Domain.Enums;
using System;

namespace SmartPark.Domain.Entities
{
    public sealed class Vehicle : AggregateRoot
    {
        public Guid CompanyId { get; private set; }
        public Plate Plate { get; private set; } = null!;
        public string? Brand { get; private set; }
        public string? Model { get; private set; }
        public string? Color { get; private set; }
        public VehicleType VehicleType { get; private set; }
        public GeneralStatus Status { get; private set; }
        private Vehicle() : base(Guid.Empty) { }
        public Vehicle(
            Guid id,
            Guid companyId,
            Plate plate,
            VehicleType vehicleType = VehicleType.Car,
            string? brand = null,
            string? model = null,
            string? color = null) : base(id)
        {
            if (companyId == Guid.Empty)
                throw new ArgumentException("O identificador da empresa (CompanyId) é obrigatório.", nameof(companyId));
            if (plate is null)
                throw new ArgumentNullException(nameof(plate), "A placa do veículo (Plate) é obrigatória.");
            CompanyId = companyId;
            Plate = plate;
            VehicleType = vehicleType;
            Brand = brand?.Trim();
            Model = model?.Trim();
            Color = color?.Trim();
            Status = GeneralStatus.Active;
        }

        public void UpdateInfo(string? brand, string? model, string? color)
        {
            UpdatedAt = DateTime.Now;
            Brand = brand?.Trim();
            Model = model?.Trim();
            Color = color?.Trim();
        }

        public void ChangeType(VehicleType vehicleType)
        {
            UpdatedAt = DateTime.Now;
            VehicleType = vehicleType;
        }

        public void Deactivate()
        {
            UpdatedAt = DateTime.Now;
            Status = GeneralStatus.Inactive;
        }

        public void Activate()
        {
            UpdatedAt = DateTime.Now;
            Status = GeneralStatus.Active;
        }
    }
}