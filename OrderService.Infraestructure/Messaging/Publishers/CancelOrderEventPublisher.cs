﻿using MassTransit;
using OrderService.Application.Events;
using OrderService.Contracts.Events;

namespace OrderService.Infraestructure.Messaging.Publishers
{
    public class CancelOrderEventPublisher : EventPublisherBase<CancelOrderEvent>, ICancelOrderEventPublisher
    {
        public CancelOrderEventPublisher(IPublishEndpoint publishEndpoint)
            : base(publishEndpoint)
        {
        }

        public Task PublishAsync(CancelOrderEvent cancelOrderEvent)
        {
            return base.PublishAsync(cancelOrderEvent);
        }
    }
}
