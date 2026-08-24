using SmartPark.Domain.Primitives;
using SmartPark.Domain.ValueObjects;
using SmartPark.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartPark.Domain.Entities
{
    public sealed class WorkOrder : AggregateRoot
    {
        private readonly List<WorkOrderItem> _items = [];
        public Guid CompanyId { get; private set; }
        public Guid? ParkingId { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid VehicleId { get; private set; }
        public string OrderNumber { get; private set; } = null!;
        public WorkOrderStatus Status { get; private set; }
        public DateTime OpenedAt { get; private set; }
        public DateTime? ClosedAt { get; private set; }
        public Money TotalAmount { get; private set; } = Money.Zero;
        public IReadOnlyCollection<WorkOrderItem> Items => _items.AsReadOnly();
        private WorkOrder() : base(Guid.Empty) { }
        public WorkOrder(
            Guid id,
            Guid companyId,
            Guid customerId,
            Guid vehicleId,
            string orderNumber,
            Guid? parkingId = null) : base(id)
        {
            if (companyId == Guid.Empty)
                throw new ArgumentException("O identificador da empresa (CompanyId) é obrigatório.", nameof(companyId));
            if (customerId == Guid.Empty)
                throw new ArgumentException("O identificador do cliente (CustomerId) é obrigatório.", nameof(customerId));
            if (vehicleId == Guid.Empty)
                throw new ArgumentException("O identificador do veículo (VehicleId) é obrigatório.", nameof(vehicleId));
            if (string.IsNullOrWhiteSpace(orderNumber))
                throw new ArgumentException("O número da ordem de serviço é obrigatório.", nameof(orderNumber));
            CompanyId = companyId;
            CustomerId = customerId;
            VehicleId = vehicleId;
            OrderNumber = orderNumber.Trim().ToUpperInvariant();
            ParkingId = parkingId;
            OpenedAt = DateTime.Now;
            Status = WorkOrderStatus.Open; 
        }
        public void AddItem(Guid itemId, Guid serviceId, decimal quantity, Money unitAmount)
        {
            if (Status == WorkOrderStatus.Completed || Status == WorkOrderStatus.Cancelled)
                throw new InvalidOperationException("Não é permitido adicionar itens a uma ordem de serviço já concluída ou cancelada.");
            var item = new WorkOrderItem(itemId, Id, serviceId, quantity, unitAmount);
            _items.Add(item);
            RecalculateTotal();
            UpdatedAt = DateTime.Now;
        }

        public void RemoveItem(Guid itemId)
        {
            if (Status == WorkOrderStatus.Completed || Status == WorkOrderStatus.Cancelled)
                throw new InvalidOperationException("Não é permitido remover itens de uma ordem de serviço já concluída ou cancelada.");
            var item = _items.Find(i => i.Id == itemId);
            if (item is not null)
            {
                _items.Remove(item);
                RecalculateTotal();
            }
            UpdatedAt = DateTime.Now;
        }

        public void StartProgress()
        {
            if (Status != WorkOrderStatus.Open)
                throw new InvalidOperationException("Apenas ordens de serviço abertas podem iniciar o atendimento.");
            UpdatedAt = DateTime.Now;
            Status = WorkOrderStatus.InProgress;
        }

        public void Complete()
        {
            if (Status == WorkOrderStatus.Completed || Status == WorkOrderStatus.Cancelled)
                throw new InvalidOperationException("Esta ordem de serviço já foi finalizada ou cancelada.");
            if (_items.Count == 0)
                throw new InvalidOperationException("Não é possível concluir uma ordem de serviço sem itens lançados.");
            UpdatedAt = DateTime.Now;
            ClosedAt = DateTime.Now;
            Status = WorkOrderStatus.Completed;
        }

        public void Cancel()
        {
            if (Status == WorkOrderStatus.Completed)
                throw new InvalidOperationException("Não é possível cancelar uma ordem de serviço que já foi concluída.");
            UpdatedAt = DateTime.Now;
            ClosedAt = DateTime.Now;
            Status = WorkOrderStatus.Cancelled;
        }

        private void RecalculateTotal()
        {
            decimal sum = 0;
            foreach (var item in _items)
            {
                sum += item.Quantity * item.UnitAmount.Amount;
            }
            TotalAmount = new Money(sum);
            UpdatedAt = DateTime.Now;
        }
    }
}