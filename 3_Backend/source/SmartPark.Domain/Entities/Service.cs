using SmartPark.Domain.Primitives;
using SmartPark.Domain.ValueObjects;
using SmartPark.Domain.Enums;
using System;

namespace SmartPark.Domain.Entities
{
    public sealed class Service : AggregateRoot
    {
        public Guid CompanyId { get; private set; }
        public Guid? ParkingId { get; private set; }
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; }
        public Money Amount { get; private set; } = Money.Zero;
        public GeneralStatus Status { get; private set; }
        private Service() : base(Guid.Empty) { }
        public Service(
            Guid id,
            Guid companyId,
            Guid? parkingId,
            string name,
            Money amount,
            string? description = null) : base(id)
        {
            if (companyId == Guid.Empty)
                throw new ArgumentException("O identificador da empresa (CompanyId) é obrigatório.", nameof(companyId));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome do serviço é obrigatório.", nameof(name));
            if (amount is null || amount.Amount < 0)
                throw new ArgumentException("O valor do serviço não pode ser negativo.", nameof(amount));
            CompanyId = companyId;
            ParkingId = parkingId;
            Name = name.Trim();
            Description = description?.Trim();
            Amount = amount;
            Status = GeneralStatus.Active; 
        }

        public void UpdateDetails(string name, string? description, Money amount)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome do serviço é obrigatório.", nameof(name));
            if (amount is null || amount.Amount < 0)
                throw new ArgumentException("O valor do serviço não pode ser negativo.", nameof(amount));
            
            Name = name.Trim();
            Description = description?.Trim();
            Amount = amount;
            UpdatedAt = DateTime.Now;
        }
        public void Deactivate()
        {
            Status = GeneralStatus.Inactive;
            UpdatedAt = DateTime.Now;
        }
        public void Activate()
        {
            Status = GeneralStatus.Active;
            UpdatedAt = DateTime.Now;
        }
    }
}