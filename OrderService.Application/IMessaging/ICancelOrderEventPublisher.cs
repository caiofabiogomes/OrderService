using OrderService.Contracts.Events;

namespace OrderService.Application.Events
{
    public interface ICancelOrderEventPublisher
    {
        Task PublishAsync(CancelOrderEvent cancelOrderEvent);
    }
}
