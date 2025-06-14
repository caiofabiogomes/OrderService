using OrderService.Contracts.Events;

namespace OrderService.Application.Events
{
    public interface IOrderCreatedEventPublisher
    {
        Task PublishAsync(CreateOrderEvent orderCreatedEvent);
    }
}
