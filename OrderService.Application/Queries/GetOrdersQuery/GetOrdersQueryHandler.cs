using OrderService.Application.Abstractions;
using OrderService.Application.Mediator;
using OrderService.Application.ViewModels;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Queries.GetOrdersQuery
{
    public class GetOrdersQueryHandler(IOrderRepository orderRepository) : IRequestHandler<GetOrdersQuery, Result<List<OrderViewModel>>>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;

        public async Task<Result<List<OrderViewModel>>> Handle(GetOrdersQuery request)
        {
            var customerId = new Guid("3FA85F64-5717-4562-B3FC-2C963F66AFA6");

            var orders = await _orderRepository.GetByCustomerIdAsync(customerId, request.Page, 10);

            List<OrderViewModel> mappedOrders = orders.Select(order => new OrderViewModel
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount.Amount,
                Status = order.OrderStatus.Status,
                Mode = order.Mode,
                OrderItems = order.OrderItems.Select(item => new OrderItemViewModel
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Title = item.Description.Title,
                    Description = item.Description.Description,
                    Quantity = item.Quantity.Value,
                    Price = item.Price.Amount
                }).ToList()
            }).ToList();

            return Result<List<OrderViewModel>>.Success(mappedOrders);
        }
    }
}
