using SmartPark.Domain.Primitives;
using System;

namespace SmartPark.Domain.Entities
{
    public sealed class StockItem : AggregateRoot
    {
        public Guid CompanyId { get; private set; }
        public Guid? ParkingId { get; private set; }
        public Guid ProductId { get; private set; }
        public decimal Quantity { get; private set; }
        public decimal MinQuantity { get; private set; }
        public string? Location { get; private set; }
        private StockItem() : base(Guid.Empty) { }
        public StockItem(
            Guid id,
            Guid companyId,
            Guid? parkingId,
            Guid productId,
            decimal quantity = 0,
            decimal minQuantity = 0,
            string? location = null) : base(id)
        {
            if (companyId == Guid.Empty)
                throw new ArgumentException("O identificador da empresa (CompanyId) é obrigatório.", nameof(companyId));
            if (productId == Guid.Empty)
                throw new ArgumentException("O identificador do produto (ProductId) é obrigatório.", nameof(productId));
            if (quantity < 0)
                throw new ArgumentException("A quantidade inicial em estoque não pode ser negativa.", nameof(quantity));
            if (minQuantity < 0)
                throw new ArgumentException("O estoque mínimo não pode ser negativo.", nameof(minQuantity));
            CompanyId = companyId;
            ParkingId = parkingId;
            ProductId = productId;
            Quantity = quantity;
            MinQuantity = minQuantity;
            Location = location?.Trim();
            UpdatedAt = DateTime.Now;
        }
        public void AddStock(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("A quantidade a ser adicionada deve ser maior que zero.", nameof(amount));
            Quantity += amount;
            UpdatedAt = DateTime.Now;
        }
        public void RemoveStock(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("A quantidade a ser removida deve ser maior que zero.", nameof(amount));
            if (Quantity - amount < 0)
                throw new InvalidOperationException("Operação negada: Estoque insuficiente para realizar a baixa.");
            Quantity -= amount;
            UpdatedAt = DateTime.Now;
        }
        public void UpdateMinQuantity(decimal minQuantity)
        {
            if (minQuantity < 0)
                throw new ArgumentException("O estoque mínimo não pode ser negativo.", nameof(minQuantity));
           
            MinQuantity = minQuantity;
            UpdatedAt = DateTime.Now;
        }
        public void UpdateLocation(string? location)
        {
            UpdatedAt = DateTime.Now;
            Location = location?.Trim();
        }
    }
}