using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Abstractions;
using OrderService.Application.Commands.PlaceOrder;
using OrderService.Application.Mediator;

namespace OrderService.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services)
        {
            services.AddScoped<IMediator, MediatorProvider>();
            services.AddScoped<IRequestHandler<PlaceOrderCommand, Result>, PlaceOrderCommandHandler>();
            return services;
        }
    }
}
