using Microsoft.Extensions.Logging;
using OrderService.Application.Abstractions;
using OrderService.Application.Mediator;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Commands.AcceptOrRejectOrder
{
    public class AcceptOrRejectOrderCommandHandler(IOrderRepository orderRepository, ILogger<AcceptOrRejectOrderCommandHandler> logger) : IRequestHandler<AcceptOrRejectOrderCommand, Result<Guid>>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly ILogger<AcceptOrRejectOrderCommandHandler> _logger = logger;

        public async Task<Result<Guid>> Handle(AcceptOrRejectOrderCommand request)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);

            if (order is null)
            {
                _logger.LogWarning("Order with ID {OrderId} not found.", request.OrderId);
                return Result<Guid>.Failure("Order not found.");
            }


            if (order.OrderStatus.Status != Domain.Enums.OrderStatus.Pending)
            {
                _logger.LogWarning("Order with ID {OrderId} is not in a valid state to be accepted or rejected. Current status: {Status}", 
                    request.OrderId, order.OrderStatus.Status);

                return Result<Guid>.Failure("Order is not in a valid state to be accepted or rejected.");
            }


            if (request.IsAccepted)
                order.MarkAsAccepted();
            else
                order.MarkAsRejected();

            await _orderRepository.SaveChangesAsync();

            _logger.LogInformation("Order with ID {OrderId} has been {Action}.",
                request.OrderId, request.IsAccepted ? "accepted" : "rejected");

            return Result<Guid>.Success(order.Id, "Order status updated successfully.");
        }
    }
}
