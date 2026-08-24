using System;

namespace SmartPark.Domain.ValueObjects
{
    public sealed record Money : IComparable<Money>
    {
        public decimal Amount { get; }
        public Money(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("O valor monetário não pode ser negativo.", nameof(amount));

            Amount = Math.Round(amount, 2);
        }

        public static Money Zero => new(0m);
        public Money Add(Money other) => new(Amount + other.Amount);
        public Money Subtract(Money other) => new(Amount - other.Amount);
        public Money Multiply(decimal multiplier) => new(Amount * multiplier);
        public static Money operator +(Money a, Money b) => a.Add(b);
        public static Money operator -(Money a, Money b) => a.Subtract(b);
        public static Money operator *(Money money, decimal multiplier) => money.Multiply(multiplier);
        public static Money operator *(decimal multiplier, Money money) => money.Multiply(multiplier);
        public int CompareTo(Money? other)
        {
            if (other is null) return 1;
            return Amount.CompareTo(other.Amount);
        }
        public static bool operator >(Money a, Money b) => a.Amount > b.Amount;
        public static bool operator <(Money a, Money b) => a.Amount < b.Amount;
        public static bool operator >=(Money a, Money b) => a.Amount >= b.Amount;
        public static bool operator <=(Money a, Money b) => a.Amount <= b.Amount;
        public override string ToString() => Amount.ToString("C2");
    }
}