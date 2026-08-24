using SmartPark.Domain.Primitives;
using SmartPark.Domain.Enums;
using System;

namespace SmartPark.Domain.Entities
{
    public sealed class Customer : AggregateRoot
    {
        public Guid CompanyId { get; private set; }
        public Guid PersonId { get; private set; }
        public CustomerType CustomerType { get; private set; }
        public GeneralStatus Status { get; private set; }
        private Customer() : base(Guid.Empty) { }
        public Customer(
            Guid id,
            Guid companyId,
            Guid personId,
            CustomerType customerType) : base(id)
        {
            if (companyId == Guid.Empty)
                throw new ArgumentException("O identificador da empresa (CompanyId) é obrigatório.", nameof(companyId));
            if (personId == Guid.Empty)
                throw new ArgumentException("O identificador da pessoa (PersonId) é obrigatório.", nameof(personId));
            CompanyId = companyId;
            PersonId = personId;
            CustomerType = customerType;
            Status = GeneralStatus.Active;
        }
        public void ChangeCustomerType(CustomerType newType)
        {
            UpdatedAt = DateTime.Now;
            CustomerType = newType;
        }
        public void Block()
        {
            UpdatedAt = DateTime.Now;
            Status = GeneralStatus.Blocked;
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