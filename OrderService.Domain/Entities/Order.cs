using OrderService.Domain.Enums;
using OrderService.Domain.ValueObjects;

namespace OrderService.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Order()
        {
        }

        public Order(Guid customerId,OrderMode mode, OrderTotalAmount totalAmount)
        {
            CustomerId = customerId;
            OrderStatus = new ValueObjects.OrderStatus();
            Mode = mode;
            OrderDate = DateTime.UtcNow;
            TotalAmount = totalAmount;
        }

        public Guid CustomerId { get; private set; }
        public ValueObjects.OrderStatus OrderStatus { get; private set; }
        public OrderMode Mode { get; private set; }
        public DateTime OrderDate { get;  init; }
        public OrderTotalAmount TotalAmount { get; private set; }

        public void MarkAsAccepted() => OrderStatus = OrderStatus.Accepted();
        public void MarkAsRejected() => OrderStatus = OrderStatus.Rejected();
        public void MarkAsCompleted() => OrderStatus = OrderStatus.Completed();
        public void MarkAsCancelled(string justification) => OrderStatus = OrderStatus.Cancelled(justification);
        public void MarkAsProcessing() => OrderStatus = OrderStatus.Processing();
    }
}
