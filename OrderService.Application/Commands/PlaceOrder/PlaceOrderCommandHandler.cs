using OrderService.Application.Abstractions;
using OrderService.Application.Events.Abstractions;
using OrderService.Application.Mediator;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;
using OrderService.Domain.ValueObjects;

namespace OrderService.Application.Commands.PlaceOrder
{
    public class PlaceOrderCommandHandler(IOrderRepository orderRepository, IOrderCreatedEventPublisher orderCreatedEventPublisher) : IRequestHandler<PlaceOrderCommand, Result<Guid>>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IOrderCreatedEventPublisher _orderCreatedEventPublisher = orderCreatedEventPublisher;

        public async Task<Result<Guid>> Handle(PlaceOrderCommand request)
        {
            if (request.Items == null || !request.Items.Any())
            {
                return Result<Guid>.Failure("Order must contain at least one item.");
            }

            var customerExists = true;

            if (!customerExists)
            {
                return Result<Guid>.Failure("Customer does not exist.");
            }

            //if(request.Items.Count != request.Items.Count)
            //{
            //    return Result<Guid>.Failure("One or more products do not exist.");
            //}


            ICollection<OrderItem> items = new List<OrderItem>();

            foreach (var item in request.Items)
            {
                items.Add(new OrderItem(item.ProductId,
                    new OrderItemQuantity(item.Quantity), 
                    new ItemDescription("teste", "description"), 
                    new Price(15.3m)));
            }


            var order = new Order(request.CustomerId, request.Mode, items);

            await _orderRepository.AddAsync(order);

           await _orderCreatedEventPublisher.PublishAsync(new Events.OrderCreatedEvent() { CustomerId = order.CustomerId});

            return Result<Guid>.Success(order.Id, "Order placed successfully.");

        }
    }
}
