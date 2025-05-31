using OrderService.Domain.Enums;

namespace OrderService.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Order()
        {
            
        }

        public Order(Guid customerId,OrderMode mode, decimal totalAmount)
        {
            CustomerId = customerId;
            Status = OrderStatus.Pending;
            Mode = mode;
            OrderDate = DateTime.UtcNow;
            TotalAmount = totalAmount;
        }

        public Guid CustomerId { get; private set; }
        public OrderStatus Status { get; private set; }
        public OrderMode Mode { get; private set; }
        public DateTime OrderDate { get; private set; }
        public decimal TotalAmount { get; private set; }
    }
}
