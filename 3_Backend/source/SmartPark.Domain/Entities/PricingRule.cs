using SmartPark.Domain.Primitives;
using SmartPark.Domain.ValueObjects;
using SmartPark.Domain.Enums;
using System;

namespace SmartPark.Domain.Entities
{
    public sealed class PricingRule : Entity
    {
        public Guid PricingTableId { get; private set; }
        public PricingRuleType RuleType { get; private set; }
        public int StartMinute { get; private set; }
        public int? EndMinute { get; private set; }
        public int? FractionMinutes { get; private set; }
        public Money Amount { get; private set; } = Money.Zero;
        public int Priority { get; private set; }
        private PricingRule() : base(Guid.Empty) { }
        internal PricingRule(
            Guid id,
            Guid pricingTableId,
            PricingRuleType ruleType,
            int startMinute,
            int? endMinute,
            int? fractionMinutes,
            Money amount,
            int priority) : base(id)
        {
            if (pricingTableId == Guid.Empty)
                throw new ArgumentException("O identificador da tabela de preços (PricingTableId) é obrigatório.", nameof(pricingTableId));
            if (startMinute < 0)
                throw new ArgumentException("O minuto inicial não pode ser negativo.", nameof(startMinute));
            if (endMinute.HasValue && endMinute.Value <= startMinute)
                throw new ArgumentException("O minuto final deve ser estritamente maior que o minuto inicial.", nameof(endMinute));
            if (fractionMinutes.HasValue && fractionMinutes.Value <= 0)
                throw new ArgumentException("Os minutos da fração devem ser maiores que zero.", nameof(fractionMinutes));
            if (amount is null || amount.Amount < 0)
                throw new ArgumentException("O valor da regra de tarifação não pode ser negativo.", nameof(amount));
            PricingTableId = pricingTableId;
            RuleType = ruleType;
            StartMinute = startMinute;
            EndMinute = endMinute;
            FractionMinutes = fractionMinutes;
            Amount = amount;
            Priority = priority;
        }
        public void UpdateRule(PricingRuleType ruleType, int startMinute, int? endMinute, int? fractionMinutes, Money amount, int priority)
        {
            if (startMinute < 0)
                throw new ArgumentException("O minuto inicial não pode ser negativo.", nameof(startMinute));
            if (endMinute.HasValue && endMinute.Value <= startMinute)
                throw new ArgumentException("O minuto final deve ser estritamente maior que o minuto inicial.", nameof(endMinute));
            if (amount is null || amount.Amount < 0)
                throw new ArgumentException("O valor da regra de tarifação não pode ser negativo.", nameof(amount));
            RuleType = ruleType;
            StartMinute = startMinute;
            EndMinute = endMinute;
            FractionMinutes = fractionMinutes;
            Amount = amount;
            Priority = priority;
            UpdatedAt = DateTime.Now;
        }
    }
}