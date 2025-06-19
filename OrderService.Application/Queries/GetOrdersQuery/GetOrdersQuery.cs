using OrderService.Application.Abstractions;
using OrderService.Application.Mediator;
using OrderService.Application.ViewModels;
using OrderService.Domain.Abstractions;

namespace OrderService.Application.Queries.GetOrdersQuery
{
    public class GetOrdersQuery : IRequest<Result<PagedResult<OrderViewModel>>>
    {
        public int Page { get; set; } = 1;

        public Guid CustomerId { get; set; } = Guid.Empty;
    }
}
