using OrderService.Domain.ValueObjects;

namespace OrderService.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public OrderItem()
        {
        }

        public OrderItem(Guid productId, OrderItemQuantity quantity,ItemDescription description, Price price)
        {
            ProductId = productId;
            Quantity = quantity;
            Description = description;
            Price = price;
        }

        public OrderItem(Guid productId, Guid orderId, OrderItemQuantity quantity,ItemDescription description, Price price)
        {
            ProductId = productId;
            OrderId = orderId;
            Quantity = quantity;
            Description = description;
            Price = price;
        }

        public Guid ProductId { get; private set; }

        public Guid OrderId { get; private set; }

        public ItemDescription Description { get; private set; }

        public Order Order { get; }

        public OrderItemQuantity Quantity { get; private set; }

        public Price Price { get; private set; }
    }
}
