using SmartPark.Domain.Primitives;
using SmartPark.Domain.ValueObjects;
using SmartPark.Domain.Enums;
using System;

namespace SmartPark.Domain.Entities
{
    public sealed class Establishment : AggregateRoot
    {
        public Guid CompanyId { get; private set; }
        public Guid? ParentEstablishmentId { get; private set; }
        public EstablishmentType EstablishmentType { get; private set; }
        public string LegalName { get; private set; } = null!;
        public string? TradeName { get; private set; }
        public TaxId TaxId { get; private set; } = null!;
        public string City { get; private set; } = null!;
        public string State { get; private set; } = null!;
        public GeneralStatus Status { get; private set; }
        private Establishment() : base(Guid.Empty) { }
        public Establishment(
            Guid id,
            Guid companyId,
            EstablishmentType establishmentType,
            string legalName,
            TaxId taxId,
            string city,
            string state,
            string? tradeName = null,
            Guid? parentId = null) : base(id)
        {
            if (companyId == Guid.Empty)
                throw new ArgumentException("O identificador da empresa (CompanyId) é obrigatório.", nameof(companyId));
            if (string.IsNullOrWhiteSpace(legalName))
                throw new ArgumentException("A razão social (LegalName) é obrigatória.", nameof(legalName));
            if (taxId is null)
                throw new ArgumentNullException(nameof(taxId), "O documento (TaxId) é obrigatório.");
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("A cidade é obrigatória.", nameof(city));
            if (string.IsNullOrWhiteSpace(state) || state.Length != 2)
                throw new ArgumentException("O estado (UF) deve conter exatamente 2 caracteres.", nameof(state));
            CompanyId = companyId;
            EstablishmentType = establishmentType;
            LegalName = legalName;
            TradeName = tradeName;
            TaxId = taxId;
            City = city.Trim();
            State = state.Trim().ToUpperInvariant();
            ParentEstablishmentId = parentId;
            Status = GeneralStatus.Active;
        }
        public void UpdateDetails(string legalName, string? tradeName, string city, string state)
        {
            if (string.IsNullOrWhiteSpace(legalName))
                throw new ArgumentException("A razão social é obrigatória.", nameof(legalName));
            LegalName = legalName;
            TradeName = tradeName;
            City = city;
            State = state.ToUpperInvariant();
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