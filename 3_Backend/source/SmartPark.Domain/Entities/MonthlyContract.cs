using SmartPark.Domain.Primitives;
using SmartPark.Domain.ValueObjects;
using SmartPark.Domain.Enums;
using System;

namespace SmartPark.Domain.Entities
{
    public sealed class MonthlyContract : AggregateRoot
    {
        public Guid CompanyId { get; private set; }
        public Guid ParkingId { get; private set; }
        public Guid CustomerId { get; private set; }
        public string ContractNumber { get; private set; } = null!;
        public DateOnly ValidFrom { get; private set; }
        public DateOnly? ValidUntil { get; private set; }
        public Money MonthlyAmount { get; private set; } = Money.Zero;
        public TimeOnly? AllowedStartTime { get; private set; }
        public TimeOnly? AllowedEndTime { get; private set; }
        public string? DaysOfWeek { get; private set; } 
        public MonthlyContractStatus Status { get; private set; }
        private MonthlyContract() : base(Guid.Empty) { }
        public MonthlyContract(
            Guid id,
            Guid companyId,
            Guid parkingId,
            Guid customerId,
            string contractNumber,
            DateOnly validFrom,
            Money monthlyAmount,
            DateOnly? validUntil = null,
            TimeOnly? allowedStartTime = null,
            TimeOnly? allowedEndTime = null) : base(id)
        {
            if (companyId == Guid.Empty)
                throw new ArgumentException("O identificador da empresa (CompanyId) é obrigatório.", nameof(companyId));
            if (parkingId == Guid.Empty)
                throw new ArgumentException("O identificador do estacionamento (ParkingId) é obrigatório.", nameof(parkingId));
            if (customerId == Guid.Empty)
                throw new ArgumentException("O identificador do cliente (CustomerId) é obrigatório.", nameof(customerId));
            if (string.IsNullOrWhiteSpace(contractNumber))
                throw new ArgumentException("O número do contrato é obrigatório.", nameof(contractNumber));
            if (validUntil.HasValue && validUntil.Value < validFrom)
                throw new ArgumentException("A data de término da vigência não pode ser anterior à data de início.", nameof(validUntil));
            if (monthlyAmount is null || monthlyAmount.Amount <= 0)
                throw new ArgumentException("O valor da mensalidade deve ser maior que zero.", nameof(monthlyAmount));
            CompanyId = companyId;
            ParkingId = parkingId;
            CustomerId = customerId;
            ContractNumber = contractNumber.Trim();
            ValidFrom = validFrom;
            ValidUntil = validUntil;
            MonthlyAmount = monthlyAmount;
            AllowedStartTime = allowedStartTime;
            AllowedEndTime = allowedEndTime;
            Status = MonthlyContractStatus.Draft;
        }
        public void Activate()
        {
            if (Status != MonthlyContractStatus.Draft && Status != MonthlyContractStatus.Suspended)
                throw new InvalidOperationException("Apenas contratos em rascunho ou suspensos podem ser ativados.");
            Status = MonthlyContractStatus.Active;
            UpdatedAt = DateTime.Now;
        }
        public void Suspend()
        {
            if (Status != MonthlyContractStatus.Active)
                throw new InvalidOperationException("Apenas contratos ativos podem ser suspensos.");
            Status = MonthlyContractStatus.Suspended;
            UpdatedAt = DateTime.Now;
        }
        public void Expire()
        {
            Status = MonthlyContractStatus.Expired;
            UpdatedAt = DateTime.Now;
        }
        public void Cancel()
        {
            if (Status == MonthlyContractStatus.Cancelled)
                throw new InvalidOperationException("Este contrato já se encontra cancelado.");
            Status = MonthlyContractStatus.Cancelled;
            UpdatedAt = DateTime.Now;
        }
    }
}