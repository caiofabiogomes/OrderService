namespace OrderService.Application.Events.Abstractions
{
    public interface IOrderCreatedEventPublisher
    {
        Task PublishAsync(OrderCreatedEvent orderCreatedEvent);
    }
}
