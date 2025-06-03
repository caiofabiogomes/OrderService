using OrderService.Application.Abstractions;
using OrderService.Application.Mediator;

namespace OrderService.Application.Commands.PlaceOrder
{
    public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand,Result>
    {
        public async Task<Result> Handle(PlaceOrderCommand request)
        {
            throw new NotImplementedException();
        }
    }
}
