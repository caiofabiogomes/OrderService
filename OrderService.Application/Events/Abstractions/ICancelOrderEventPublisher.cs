namespace OrderService.Application.Events.Abstractions
{
    public interface ICancelOrderEventPublisher
    {
        Task PublishAsync(CancelOrderEvent cancelOrderEvent);
    }
}
