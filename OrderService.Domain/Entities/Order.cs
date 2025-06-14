
using OrderService.Domain.Enums;
using OrderService.Domain.Exceptions;
using OrderService.Domain.ValueObjects;

namespace OrderService.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Order()
        {
        }

        public Order(Guid customerId, OrderMode mode, ICollection<OrderItem> orderItems)
        {
            AddItems(orderItems);

            CustomerId = customerId;
            OrderStatus = new ValueObjects.OrderStatus();
            Mode = mode;
            OrderDate = DateTime.UtcNow;
        }

        public Guid CustomerId { get; private set; }

        public ValueObjects.OrderStatus OrderStatus { get; private set; }

        public OrderMode Mode { get; private set; }

        public DateTime OrderDate { get; init; }

        public Price TotalAmount { get; private set; }

        public ICollection<OrderItem> OrderItems { get; private set; }

        public void MarkAsAccepted() => OrderStatus = OrderStatus.Accepted();

        public void MarkAsRejected() => OrderStatus = OrderStatus.Rejected();

        public void MarkAsCompleted() => OrderStatus = OrderStatus.Completed();

        public void MarkAsCancelled(string justification) => OrderStatus = OrderStatus.Cancelled(justification);

        public void MarkAsProcessing() => OrderStatus = OrderStatus.Processing();

        private static void ValidateItens(ICollection<OrderItem> orderItems)
        {
            if (orderItems == null || !orderItems.Any())
                throw new DomainException(DomainExceptionMessages.InvalidOrderItems);
            if (orderItems.Any(item => item == null))
                throw new DomainException(DomainExceptionMessages.InvalidOrderItem);
            if (orderItems.GroupBy(item => item.ProductId).Any(g => g.Count() > 1))
                throw new DomainException(DomainExceptionMessages.DuplicatedOrderItem);
        }

        private void AddItems(ICollection<OrderItem> orderItems)
        {
            OrderItems ??= new List<OrderItem>();

            ValidateItens(orderItems);

            foreach (var item in orderItems)
            {
                OrderItems.Add(new OrderItem(item.ProductId,
                    Id, item.Quantity, item.Description, item.Price));

                TotalAmount = new Price((TotalAmount?.Amount ?? decimal.Zero) + item.Price.Amount * item.Quantity.Value);
            }
        }
    }
}
