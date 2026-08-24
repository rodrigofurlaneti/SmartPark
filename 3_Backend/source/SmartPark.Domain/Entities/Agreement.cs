using SmartPark.Domain.Primitives;
using SmartPark.Domain.Enums;

namespace SmartPark.Domain.Entities
{
    public sealed class Agreement : AggregateRoot
    {
        public Guid CompanyId { get; private set; }
        public Guid ParkingId { get; private set; }
        public Guid? CustomerId { get; private set; }
        public string Name { get; private set; } = null!;
        public string PartnerName { get; private set; } = null!;
        public DiscountType DiscountType { get; private set; }
        public decimal? DiscountValue { get; private set; }
        public DateTime ValidFrom { get; private set; }
        public DateTime? ValidUntil { get; private set; }
        public GeneralStatus Status { get; private set; } = GeneralStatus.Active;
        private Agreement() : base(Guid.Empty) { }
        public Agreement(
            Guid id,
            Guid companyId,
            Guid parkingId,
            string name,
            string partnerName,
            DiscountType discountType,
            decimal? discountValue,
            DateTime validFrom,
            Guid? customerId = null) : base(id)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome do convênio é obrigatório.", nameof(name));
            if (string.IsNullOrWhiteSpace(partnerName))
                throw new ArgumentException("O nome do parceiro é obrigatório.", nameof(partnerName));
            if (discountType != DiscountType.Free && (!discountValue.HasValue || discountValue.Value <= 0))
                throw new ArgumentException("O valor do desconto deve ser informado e ser maior que zero.");
            CompanyId = companyId;
            ParkingId = parkingId;
            CustomerId = customerId;
            Name = name;
            PartnerName = partnerName;
            DiscountType = discountType;
            DiscountValue = discountType == DiscountType.Free ? 0m : discountValue;
            ValidFrom = validFrom;
            Status = GeneralStatus.Active;
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