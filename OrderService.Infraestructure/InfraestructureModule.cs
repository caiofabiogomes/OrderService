using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OrderService.Application.Abstractions;
using OrderService.Application.Commands.AcceptOrRejectOrder;
using OrderService.Application.Events;
using OrderService.Application.ExternalServices;
using OrderService.Application.Mediator;
using OrderService.Contracts.Events;
using OrderService.Domain.Repositories;
using OrderService.Infraestructure.Messaging.Publishers;
using OrderService.Infraestructure.Persistence;
using OrderService.Infraestructure.Persistence.Repositories;
using OrderService.Infrastructure.ExternalServices;
using OrderService.Infrastructure.Messaging.Consumers;
using System.Text;

namespace OrderService.Infraestructure
{
    public static class InfraestructureModule
    {
        public static IServiceCollection AddInfraestructureModule(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAutentication(configuration)
                .AddPersistence(configuration)
                .AddExternalServices()
                .AddConsumersCommands()
                .AddRepositories()
                .AddMessaging();

            return services;
        }

        private static IServiceCollection AddAutentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtKey = configuration["Jwt:Key"];

            services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(jwtKey)
                    )
                };
            });

            services.AddAuthorization();

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
        
        private static IServiceCollection AddConsumersCommands(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<AcceptOrRejectOrderCommand, Result<Guid>>, AcceptOrRejectOrderCommandHandler>();
            return services;
        }

        private static IServiceCollection AddExternalServices(this IServiceCollection services) 
        {
            var envService = Environment.GetEnvironmentVariable("URL_PRODUCTS_API") ?? "http://localhost:32000/menu/";

            services.AddHttpClient<IGetItemsOrderService, GetItemsOrderService>(client =>
            {
                client.BaseAddress = new Uri(envService); 
            });

            return services;
        }

        private static IServiceCollection AddMessaging(this IServiceCollection services)
        {
            var envHostRabbitMqServer = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "rabbitmq://localhost:31001";

            services.AddMassTransit(x =>
            {
                x.AddConsumer<AcceptOrRejectOrderEventConsumer>();

                x.AddEntityFrameworkOutbox<OrderServiceDBContext>(o =>
                {
                    o.UseSqlServer();
                    o.UseBusOutbox();
                });

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Message<CreateOrderEvent>(x =>
                    {
                        x.SetEntityName("order-created-event");
                    });

                    cfg.Message<CancelOrderEvent>(x =>
                    {
                        x.SetEntityName("cancel-order-event");
                    });

                    cfg.ReceiveEndpoint("accept-or-reject-order-event", e =>
                    {
                        e.ConfigureConsumer<AcceptOrRejectOrderEventConsumer>(context);
                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                    });

                    cfg.Host(envHostRabbitMqServer);
                });

            });
            services.AddScoped<IOrderCreatedEventPublisher, OrderCreatedEventPublisher>();
            services.AddScoped<ICancelOrderEventPublisher, CancelOrderEventPublisher>();
            return services;
        }
    }
}
