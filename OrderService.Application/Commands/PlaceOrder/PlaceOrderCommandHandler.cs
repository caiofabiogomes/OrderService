using AutoMapper;
using OrderService.Application.Abstractions;
using OrderService.Application.Events;
using OrderService.Application.ExternalServices;
using OrderService.Application.Mediator;
using OrderService.Contracts.Events;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;
using OrderService.Domain.ValueObjects;

namespace OrderService.Application.Commands.PlaceOrder
{
    public class PlaceOrderCommandHandler(IOrderRepository orderRepository,
                                          IOrderCreatedEventPublisher orderCreatedEventPublisher,
                                          IGetItemsOrderService getItemsOrderService,
                                          IMapper mapper) : IRequestHandler<PlaceOrderCommand, Result<Guid>>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IOrderCreatedEventPublisher _orderCreatedEventPublisher = orderCreatedEventPublisher;
        private readonly IGetItemsOrderService _getItemsOrderService = getItemsOrderService;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<Guid>> Handle(PlaceOrderCommand request)
        {
            if (request.Items == null || !request.Items.Any())
            {
                return Result<Guid>.Failure("Order must contain at least one item.");
            }

            var items = await _getItemsOrderService.GetItemsAsync(request.Items.Select(i => i.ProductId).ToList(), request.Token);

            if (!items.IsSuccess)
                return Result<Guid>.Failure("Failed to Process Order. An error ocurred to get the items");

            if (items.Data == null || !items.Data.Any())
            {
                return Result<Guid>.Failure("No items found for the provided product IDs.");
            }

            var existingProductIds = items.Data.Select(c => c.Id).ToHashSet();

            if (!request.Items.All(x => existingProductIds.Contains(x.ProductId)))
            {
                return Result<Guid>.Failure("One or more products doesn't exists");
            }

            ICollection<OrderItem> itemsOrder = new List<OrderItem>();

            foreach (var item in items.Data)
            {
                if(!item.IsAvailable)
                    return Result<Guid>.Failure($"Product {item.Name} is not available for purchase.");

                if (string.IsNullOrWhiteSpace(item.Description))
                    item.Description = "Empty Description";

                var requestItem = request.Items.FirstOrDefault(x => x.ProductId == item.Id)!;

                itemsOrder.Add(new OrderItem(item.Id,
                    new OrderItemQuantity(requestItem.Quantity),
                    new ItemDescription(item.Name, item.Description),
                    new Price(item.Price)));
            }

            var order = new Order(request.CustomerId, request.Mode, itemsOrder);

            await _orderRepository.AddWithoutSaveChangesAsync(order);

            var createOrderDto = _mapper.Map<CreateOrderEvent>(order);

            await _orderCreatedEventPublisher.PublishAsync(createOrderDto);

            await _orderRepository.SaveChangesAsync();

            return Result<Guid>.Success(order.Id, "Order placed successfully.");
        }
    }
}
