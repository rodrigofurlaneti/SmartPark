using SmartPark.Domain.Primitives;
using SmartPark.Domain.Enums;
using System;

namespace SmartPark.Domain.Entities
{
    public sealed class FiscalConfiguration : AggregateRoot
    {
        public Guid CompanyId { get; private set; }
        public Guid EstablishmentId { get; private set; }
        public string? MunicipalRegistration { get; private set; }
        public string? TaxRegime { get; private set; }
        public string? ServiceCode { get; private set; }
        public string? Provider { get; private set; }
        public GeneralStatus Status { get; private set; }
        private FiscalConfiguration() : base(Guid.Empty) { }
        public FiscalConfiguration(
            Guid id,
            Guid companyId,
            Guid establishmentId,
            string? municipalRegistration = null,
            string? taxRegime = null,
            string? serviceCode = null,
            string? provider = null) : base(id)
        {
            if (companyId == Guid.Empty)
                throw new ArgumentException("O identificador da empresa (CompanyId) é obrigatório.", nameof(companyId));
            if (establishmentId == Guid.Empty)
                throw new ArgumentException("O identificador do estabelecimento (EstablishmentId) é obrigatório.", nameof(establishmentId));
            CompanyId = companyId;
            EstablishmentId = establishmentId;
            MunicipalRegistration = municipalRegistration;
            TaxRegime = taxRegime;
            ServiceCode = serviceCode;
            Provider = provider;
            Status = GeneralStatus.Active;
        }
        public void UpdateDetails(string? municipalRegistration, string? taxRegime, string? serviceCode, string? provider)
        {
            UpdatedAt = DateTime.Now;
            MunicipalRegistration = municipalRegistration;
            TaxRegime = taxRegime;
            ServiceCode = serviceCode;
            Provider = provider;
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