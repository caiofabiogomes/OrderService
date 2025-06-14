using OrderService.Application.Abstractions;
using OrderService.Application.Mediator;
using OrderService.Application.ViewModels;

namespace OrderService.Application.Queries.GetOrdersQuery
{
    public class GetOrdersQuery : IRequest<Result<List<OrderViewModel>>>
    {
        public int Page { get; set; } = 1;
    }
}
