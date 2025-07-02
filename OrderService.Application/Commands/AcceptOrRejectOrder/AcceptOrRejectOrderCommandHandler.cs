using OrderService.Application.Abstractions;
using OrderService.Application.Mediator;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Commands.AcceptOrRejectOrder
{
    public class AcceptOrRejectOrderCommandHandler(IOrderRepository orderRepository) : IRequestHandler<AcceptOrRejectOrderCommand, Result<Guid>>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        public async Task<Result<Guid>> Handle(AcceptOrRejectOrderCommand request)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);

            if(order is null)
                return Result<Guid>.Failure("Order not found.");

            if(order.OrderStatus.Status != Domain.Enums.OrderStatus.Pending)
                return Result<Guid>.Failure("Order is not in a valid state to be accepted or rejected.");

            if (request.IsAccepted)
                order.MarkAsAccepted();
            else
                order.MarkAsRejected();

            await _orderRepository.UpdateAsync(order);

            return Result<Guid>.Success(order.Id, "Order status updated successfully.");
        }
    }
}
