using SmartPark.Domain.Primitives;
using SmartPark.Domain.ValueObjects;
using SmartPark.Domain.Enums;
using System;

namespace SmartPark.Domain.Entities
{
    public sealed class Product : AggregateRoot
    {
        public Guid CompanyId { get; private set; }
        public string Sku { get; private set; } = null!;
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; }
        public ProductUnit Unit { get; private set; }
        public Money CostAmount { get; private set; } = Money.Zero;
        public Money SaleAmount { get; private set; } = Money.Zero;
        public GeneralStatus Status { get; private set; }
        private Product() : base(Guid.Empty) { }
        public Product(
            Guid id,
            Guid companyId,
            string sku,
            string name,
            Money costAmount,
            Money saleAmount,
            string? description = null,
            ProductUnit unit = ProductUnit.Un) : base(id)
        {
            if (companyId == Guid.Empty)
                throw new ArgumentException("O identificador da empresa (CompanyId) é obrigatório.", nameof(companyId));
            if (string.IsNullOrWhiteSpace(sku))
                throw new ArgumentException("O código SKU do produto é obrigatório.", nameof(sku));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome do produto é obrigatório.", nameof(name));
            if (costAmount is null || costAmount.Amount < 0)
                throw new ArgumentException("O preço de custo não pode ser negativo.", nameof(costAmount));
            if (saleAmount is null || saleAmount.Amount < 0)
                throw new ArgumentException("O preço de venda não pode ser negativo.", nameof(saleAmount));
            CompanyId = companyId;
            Sku = sku.Trim().ToUpperInvariant();
            Name = name.Trim();
            Description = description?.Trim();
            Unit = unit;
            CostAmount = costAmount;
            SaleAmount = saleAmount;
            Status = GeneralStatus.Active;
        }
        public void UpdateDetails(string name, string? description, ProductUnit unit, Money costAmount, Money saleAmount)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome do produto é obrigatório.", nameof(name));
            if (costAmount is null || costAmount.Amount < 0)
                throw new ArgumentException("O preço de custo não pode ser negativo.", nameof(costAmount));
            if (saleAmount is null || saleAmount.Amount < 0)
                throw new ArgumentException("O preço de venda não pode ser negativo.", nameof(saleAmount));
            Name = name.Trim();
            Description = description?.Trim();
            Unit = unit;
            CostAmount = costAmount;
            SaleAmount = saleAmount;
            UpdatedAt = DateTime.Now;
        }
        public void Deactivate()
        {
            Status = GeneralStatus.Inactive;
            UpdatedAt = DateTime.Now;
        }
        public void Activate()
        {
            Status = GeneralStatus.Active;
            UpdatedAt = DateTime.Now;
        }
    }
}