using System;
using System.Text.RegularExpressions;

namespace SmartPark.Domain.ValueObjects
{
    public sealed record Plate
    {
        public string Value { get; }
        public Plate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("A placa do veículo não pode ser vazia.", nameof(value));
            var cleanValue = Regex.Replace(value.Trim().ToUpperInvariant(), "[^A-Z0-9]", "");
            if (cleanValue.Length < 7 || cleanValue.Length > 8)
                throw new ArgumentException("A placa do veículo deve conter entre 7 e 8 caracteres válidos.", nameof(value));
            Value = cleanValue;
        }
        public override string ToString() => Value;
    }
}