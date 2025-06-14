using OrderService.Domain.Enums;

namespace OrderService.Application.ViewModels
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Domain.Enums.OrderStatus Status { get; set; }
        public OrderMode Mode { get; set; }
        public DateTime OrderDate { get; init; }
        public Decimal TotalAmount { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; }
    }

    public class OrderItemViewModel 
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
