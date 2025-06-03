using OrderService.Application.Abstractions;
using OrderService.Application.Mediator;
using OrderService.Domain.Enums;

namespace OrderService.Application.Commands.PlaceOrder
{
    public class PlaceOrderCommand : IRequest<Result>
    {
        public Guid CustomerId { get; set; }
         
        public OrderMode Mode { get; set; }

        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}
