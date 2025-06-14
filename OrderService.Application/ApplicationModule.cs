using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Abstractions;
using OrderService.Application.Commands.CancelOrder;
using OrderService.Application.Commands.PlaceOrder;
using OrderService.Application.Mediator;
using OrderService.Application.Queries.GetOrdersQuery;
using OrderService.Application.ViewModels;

namespace OrderService.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services)
        {
            services.AddScoped<IMediator, MediatorProvider>();
            services.AddScoped<IRequestHandler<PlaceOrderCommand, Result<Guid>>, PlaceOrderCommandHandler>();
            services.AddScoped<IRequestHandler<CancelOrderCommand, Result<Guid>>, CancelOrderCommandHandler>();
            services.AddScoped<IRequestHandler<GetOrdersQuery, Result<List<OrderViewModel>>>, GetOrdersQueryHandler>();
            return services;
        }
    }
}
