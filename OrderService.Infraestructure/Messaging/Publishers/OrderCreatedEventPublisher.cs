using MassTransit;
using OrderService.Application.Events;
using OrderService.Contracts.Events;

namespace OrderService.Infraestructure.Messaging.Publishers
{
    public class OrderCreatedEventPublisher : EventPublisherBase<CreateOrderEvent>, IOrderCreatedEventPublisher
    {
        public OrderCreatedEventPublisher(IPublishEndpoint publishEndpoint)
            : base(publishEndpoint)
        {
        }

        public Task PublishAsync(CreateOrderEvent orderCreatedEvent)
        {
            return base.PublishAsync(orderCreatedEvent);
        }
    }
}
