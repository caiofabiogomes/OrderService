using MassTransit;
using OrderService.Application.Events;
using OrderService.Application.Events.Abstractions;

namespace OrderService.Infraestructure.Messaging.Publishers
{
    public class OrderCreatedEventPublisher : EventPublisherBase<OrderCreatedEvent>, IOrderCreatedEventPublisher
    {
        public OrderCreatedEventPublisher(IPublishEndpoint publishEndpoint)
            : base(publishEndpoint)
        {
        }

        public Task PublishAsync(OrderCreatedEvent orderCreatedEvent)
        {
            return base.PublishAsync(orderCreatedEvent);
        }
    }
}
