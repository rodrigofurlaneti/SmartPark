using SmartPark.Domain.Primitives;
using SmartPark.Domain.ValueObjects;
using SmartPark.Domain.Enums;
using System;

namespace SmartPark.Domain.Entities
{
    public sealed class Person : AggregateRoot
    {
        public PersonType PersonType { get; private set; }
        public string Name { get; private set; } = null!;
        public TaxId TaxId { get; private set; } = null!;
        public string? Email { get; private set; }
        public string? Phone { get; private set; }
        public string? PostalCode { get; private set; }
        public string? Street { get; private set; }
        public string? Number { get; private set; }
        public string? Complement { get; private set; }
        public string? Neighborhood { get; private set; }
        public string City { get; private set; } = null!;
        public string State { get; private set; } = null!;
        public string Country { get; private set; } = "BR";
        private Person() : base(Guid.Empty) { }
        public Person(
            Guid id,
            PersonType personType,
            string name,
            TaxId taxId,
            string city,
            string state,
            string? email = null,
            string? phone = null,
            string? postalCode = null,
            string? street = null,
            string? number = null,
            string? complement = null,
            string? neighborhood = null) : base(id)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome da pessoa é obrigatório.", nameof(name));
            if (taxId is null)
                throw new ArgumentNullException(nameof(taxId), "O documento (TaxId) é obrigatório.");
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("A cidade é obrigatória.", nameof(city));
            if (string.IsNullOrWhiteSpace(state) || state.Length != 2)
                throw new ArgumentException("O estado (UF) deve conter exatamente 2 caracteres.", nameof(state));
            PersonType = personType;
            Name = name.Trim();
            TaxId = taxId;
            City = city.Trim();
            State = state.Trim().ToUpperInvariant();
            Email = email?.Trim();
            Phone = phone?.Trim();
            PostalCode = postalCode?.Trim();
            Street = street?.Trim();
            Number = number?.Trim();
            Complement = complement?.Trim();
            Neighborhood = neighborhood?.Trim();
            Country = "BR";
        }

        public void UpdateContactInfo(string? email, string? phone)
        {
            Email = email?.Trim();
            Phone = phone?.Trim();
            UpdatedAt = DateTime.Now;
        }
        public void UpdateAddress(string? postalCode, string? street, string? number, string? complement, string? neighborhood, string city, string state)
        {
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("A cidade é obrigatória.", nameof(city));
            if (string.IsNullOrWhiteSpace(state) || state.Length != 2)
                throw new ArgumentException("O estado (UF) deve conter exatamente 2 caracteres.", nameof(state));
            PostalCode = postalCode?.Trim();
            Street = street?.Trim();
            Number = number?.Trim();
            Complement = complement?.Trim();
            Neighborhood = neighborhood?.Trim();
            City = city.Trim();
            State = state.Trim().ToUpperInvariant();
            UpdatedAt = DateTime.Now;
        }
    }
}