using System;
using System.Linq;

namespace SmartPark.Domain.ValueObjects
{
    public sealed record TaxId
    {
        public string Value { get; }
        public TaxId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("O documento (TaxId) não pode ser vazio.", nameof(value));
            Value = new string(value.Where(char.IsDigit).ToArray());
            if (Value.Length != 11 && Value.Length != 14)
                throw new ArgumentException("O documento deve ser um CPF (11 dígitos) ou CNPJ (14 dígitos).", nameof(value));
            if (Value.Length == 11 && !IsValidCpf(Value))
                throw new ArgumentException("O CPF informado é inválido.", nameof(value));
            if (Value.Length == 14 && !IsValidCnpj(Value))
                throw new ArgumentException("O CNPJ informado é inválido.", nameof(value));
        }

        private static bool IsValidCpf(string cpf)
        {
            if (cpf.Distinct().Count() == 1) return false;
            int[] multiplier1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
            int[] multiplier2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];
            string tempCpf = cpf[..9];
            int sum = tempCpf.Zip(multiplier1, (digit, mult) => (digit - '0') * mult).Sum();
            int remainder = sum % 11;
            remainder = remainder < 2 ? 0 : 11 - remainder;
            string digit = remainder.ToString();
            tempCpf += digit;
            sum = tempCpf.Zip(multiplier2, (digit, mult) => (digit - '0') * mult).Sum();
            remainder = sum % 11;
            remainder = remainder < 2 ? 0 : 11 - remainder;
            digit += remainder;
            return cpf.EndsWith(digit);
        }

        private static bool IsValidCnpj(string cnpj)
        {
            if (cnpj.Distinct().Count() == 1) return false;
            int[] multiplier1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
            int[] multiplier2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
            string tempCnpj = cnpj[..12];
            int sum = tempCnpj.Zip(multiplier1, (digit, mult) => (digit - '0') * mult).Sum();
            int remainder = sum % 11;
            remainder = remainder < 2 ? 0 : 11 - remainder;
            string digit = remainder.ToString();
            tempCnpj += digit;
            sum = tempCnpj.Zip(multiplier2, (digit, mult) => (digit - '0') * mult).Sum();
            remainder = sum % 11;
            remainder = remainder < 2 ? 0 : 11 - remainder;
            digit += remainder;
            return cnpj.EndsWith(digit);
        }
        public override string ToString() => Value;
    }
}