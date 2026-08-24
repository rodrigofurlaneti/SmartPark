using SmartPark.Domain.Primitives;
using SmartPark.Domain.ValueObjects;
using SmartPark.Domain.Enums;
using System;
using System.Collections.Generic;

namespace SmartPark.Domain.Entities
{
    public sealed class PricingTable : AggregateRoot
    {
        private readonly List<PricingRule> _rules = [];
        public Guid CompanyId { get; private set; }
        public Guid ParkingId { get; private set; }
        public string Name { get; private set; } = null!;
        public OperationType OperationType { get; private set; }
        public DateTime ValidFrom { get; private set; }
        public DateTime? ValidUntil { get; private set; }
        public PricingTableStatus Status { get; private set; }
        public IReadOnlyCollection<PricingRule> Rules => _rules.AsReadOnly();
        private PricingTable() : base(Guid.Empty) { }
        public PricingTable(
            Guid id,
            Guid companyId,
            Guid parkingId,
            string name,
            DateTime validFrom,
            OperationType operationType = OperationType.Rotative,
            DateTime? validUntil = null) : base(id)
        {
            if (companyId == Guid.Empty)
                throw new ArgumentException("O identificador da empresa (CompanyId) é obrigatório.", nameof(companyId));
            if (parkingId == Guid.Empty)
                throw new ArgumentException("O identificador do estacionamento (ParkingId) é obrigatório.", nameof(parkingId));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome da tabela de preços é obrigatório.", nameof(name));
            if (validUntil.HasValue && validUntil.Value <= validFrom)
                throw new ArgumentException("A data de término da validade deve ser estritamente posterior à data de início.", nameof(validUntil));
            CompanyId = companyId;
            ParkingId = parkingId;
            Name = name.Trim();
            OperationType = operationType;
            ValidFrom = validFrom;
            ValidUntil = validUntil;
            Status = PricingTableStatus.Draft;
        }
        public void AddRule(
            Guid ruleId,
            PricingRuleType ruleType,
            int startMinute,
            int? endMinute,
            int? fractionMinutes,
            Money amount,
            int priority)
        {
            if (Status == PricingTableStatus.Active)
                throw new InvalidOperationException("Não é permitido adicionar regras a uma tabela de preços que já está ativa. Crie uma nova versão ou coloque em rascunho.");
            var rule = new PricingRule(ruleId, Id, ruleType, startMinute, endMinute, fractionMinutes, amount, priority);
            _rules.Add(rule);
            UpdatedAt = DateTime.Now;
        }
        public void RemoveRule(Guid ruleId)
        {
            if (Status == PricingTableStatus.Active)
                throw new InvalidOperationException("Não é permitido remover regras de uma tabela de preços ativa.");
            var rule = _rules.Find(r => r.Id == ruleId);
            if (rule is not null)
            {
                _rules.Remove(rule);
            }
            UpdatedAt = DateTime.Now;
        }
        public void Activate()
        {
            if (_rules.Count == 0)
                throw new InvalidOperationException("Não é possível ativar uma tabela de preços que não possui regras cadastradas.");
            Status = PricingTableStatus.Active;
            UpdatedAt = DateTime.Now;
        }
        public void Deactivate()
        {
            Status = PricingTableStatus.Inactive;
            UpdatedAt = DateTime.Now;
        }
    }
}