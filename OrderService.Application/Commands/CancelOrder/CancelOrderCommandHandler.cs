using AutoMapper;
using OrderService.Application.Abstractions;
using OrderService.Application.Events;
using OrderService.Application.Mediator;
using OrderService.Contracts.Events;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Commands.CancelOrder
{
    public class CancelOrderCommandHandler(IOrderRepository orderRepository,
                                           ICancelOrderEventPublisher cancelOrderEventPublisher,
                                           IMapper mapper) : IRequestHandler<CancelOrderCommand, Result<Guid>>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly ICancelOrderEventPublisher _cancelOrderEventPublisher = cancelOrderEventPublisher;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<Guid>> Handle(CancelOrderCommand request)
        {
            if (request.OrderId == Guid.Empty) 
                return Result<Guid>.Failure("Order ID cannot be empty."); 
            
            if (string.IsNullOrWhiteSpace(request.Justification)) 
                return Result<Guid>.Failure("Justification is required for cancelling an order."); 

            var order = await _orderRepository.GetByCustomerIdAndOrderIdAsync(request.CustomerId, request.OrderId);
            
            if (order == null) 
                return Result<Guid>.Failure("Order not found."); 

            if (order.OrderStatus.Status != Domain.Enums.OrderStatus.Pending)
                return Result<Guid>.Failure("Only pending orders can be cancelled.");

            order.MarkAsCancelled(request.Justification);

            var cancelOrderEvent = new CancelOrderEvent()
            {
                OrderId = order.Id,
                Justification = request.Justification,
            };

            await _cancelOrderEventPublisher.PublishAsync(cancelOrderEvent);

            await _orderRepository.SaveChangesAsync();

            return Result<Guid>.Success(order.Id);
        }
    }
}
