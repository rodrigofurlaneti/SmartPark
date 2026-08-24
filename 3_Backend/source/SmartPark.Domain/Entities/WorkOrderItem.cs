using SmartPark.Domain.Primitives;
using SmartPark.Domain.ValueObjects;
using System;

namespace SmartPark.Domain.Entities
{
    public sealed class WorkOrderItem : Entity
    {
        public Guid WorkOrderId { get; private set; }
        public Guid ServiceId { get; private set; }
        public decimal Quantity { get; private set; }
        public Money UnitAmount { get; private set; } = Money.Zero;
        public Money TotalAmount { get; private set; } = Money.Zero;
        private WorkOrderItem() : base(Guid.Empty) { }
        internal WorkOrderItem(
            Guid id,
            Guid workOrderId,
            Guid serviceId,
            decimal quantity,
            Money unitAmount) : base(id)
        {
            if (workOrderId == Guid.Empty)
                throw new ArgumentException("O identificador da ordem de serviço (WorkOrderId) é obrigatório.", nameof(workOrderId));
            if (serviceId == Guid.Empty)
                throw new ArgumentException("O identificador do serviço (ServiceId) é obrigatório.", nameof(serviceId));
            if (quantity <= 0)
                throw new ArgumentException("A quantidade do item deve ser maior que zero.", nameof(quantity));
            if (unitAmount is null || unitAmount.Amount < 0)
                throw new ArgumentException("O valor unitário não pode ser negativo.", nameof(unitAmount));
            WorkOrderId = workOrderId;
            ServiceId = serviceId;
            Quantity = quantity;
            UnitAmount = unitAmount;
            TotalAmount = new Money(unitAmount.Amount * quantity);
            
        }
        internal void UpdateQuantityAndAmount(decimal quantity, Money unitAmount)
        {
            if (quantity <= 0)
                throw new ArgumentException("A quantidade do item deve ser maior que zero.", nameof(quantity));
            if (unitAmount is null || unitAmount.Amount < 0)
                throw new ArgumentException("O valor unitário não pode ser negativo.", nameof(unitAmount));
            UpdatedAt = DateTime.Now;
            Quantity = quantity;
            UnitAmount = unitAmount;
            TotalAmount = new Money(unitAmount.Amount * quantity);
        }
    }
}