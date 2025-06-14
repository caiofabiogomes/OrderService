using MassTransit;

namespace OrderService.Infraestructure.Messaging.Publishers
{
    public abstract class EventPublisherBase<T>
        where T : class
    {
        private readonly IPublishEndpoint _publishEndpoint;

        protected EventPublisherBase(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        protected Task PublishAsync(T @event)
        {
            return _publishEndpoint.Publish(@event);
        }
    }
}
