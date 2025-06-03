using OrderService.Domain.ValueObjects;

namespace OrderService.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public OrderItem()
        {
        }

        public OrderItem(Guid productId, Guid orderId, OrderItemQuantity quantity, Price price)
        {
            ProductId = productId;
            OrderId = orderId;
            Quantity = quantity;
            Price = price;
        }

        public Guid ProductId { get; private set; }

        public Guid OrderId { get; private set; }

        public OrderItemQuantity Quantity { get; private set; }

        public Price Price { get; private set; }
    }
}
