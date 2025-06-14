using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Events;
using OrderService.Application.Events.Abstractions;
using OrderService.Domain.Repositories;
using OrderService.Infraestructure.Messaging.Publishers;
using OrderService.Infraestructure.Persistence;
using OrderService.Infraestructure.Persistence.Repositories;

namespace OrderService.Infraestructure
{
    public static class InfraestructureModule
    {
        public static IServiceCollection AddInfraestructureModule(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddPersistence(configuration)
                .AddRepositories()
                .AddMessaging();

            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_DATABASE") ??
                configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<OrderServiceDBContext>(options => options.UseSqlServer(connectionString));
            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            return services;
        }

        private static IServiceCollection AddMessaging(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Message<OrderCreatedEvent>(x =>
                    {
                        x.SetEntityName("order-created-event");
                    });

                    cfg.Message<CancelOrderEvent>(x =>
                    {
                        x.SetEntityName("cancel-order-event");
                    });

                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                });

            });
            services.AddScoped<IOrderCreatedEventPublisher, OrderCreatedEventPublisher>();
            services.AddScoped<ICancelOrderEventPublisher, CancelOrderEventPublisher>();
            return services;
        }
    }
}
