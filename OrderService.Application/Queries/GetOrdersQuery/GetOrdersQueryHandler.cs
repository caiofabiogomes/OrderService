using AutoMapper;
using OrderService.Application.Abstractions;
using OrderService.Application.Mediator;
using OrderService.Application.ViewModels;
using OrderService.Domain.Abstractions;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Queries.GetOrdersQuery
{
    public class GetOrdersQueryHandler(IOrderRepository orderRepository, IMapper mapper) : IRequestHandler<GetOrdersQuery, Result<PagedResult<OrderViewModel>>>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<PagedResult<OrderViewModel>>> Handle(GetOrdersQuery request)
        {
            var customerId = new Guid("3FA85F64-5717-4562-B3FC-2C963F66AFA6");

            var orders = await _orderRepository.GetByCustomerIdAsync(customerId, request.Page, 10);

            List<OrderViewModel> mappedOrders = _mapper.Map<List<OrderViewModel>>(orders.Items);
            
            var result = new PagedResult<OrderViewModel>
            {
                Items = mappedOrders,
                CurrentPage = orders.CurrentPage,
                TotalPages = orders.TotalPages
            };

            return Result<PagedResult<OrderViewModel>>.Success(result, "Orders retrieved successfully.");
        }
    }
}
