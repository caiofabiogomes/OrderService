using MassTransit;
using OrderService.Application.Commands.AcceptOrRejectOrder;
using OrderService.Application.Mediator;
using OrderService.Contracts.Events;
using OrderService.Contracts.Enums;

namespace OrderService.Infrastructure.Messaging.Consumers
{
    public class AcceptOrRejectOrderEventConsumer : IConsumer<AcceptOrRejectOrderEvent>
    {
        private readonly IMediator _mediator;

        public AcceptOrRejectOrderEventConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<AcceptOrRejectOrderEvent> context)
        {
            var message = context.Message;
            var command = new AcceptOrRejectOrderCommand(message.OrderId, 
                message.Status == AcceptOrRejectOrderEnum.Accepted);

            await _mediator.Send(command);
        }
    }
}
