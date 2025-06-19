using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OrderService.Application.Events;
using OrderService.Contracts.Events;
using OrderService.Domain.Repositories;
using OrderService.Infraestructure.Messaging.Publishers;
using OrderService.Infraestructure.Persistence;
using OrderService.Infraestructure.Persistence.Repositories;
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

        private static IServiceCollection AddMessaging(this IServiceCollection services)
        {
            var envHostRabbitMqServer = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";

            services.AddMassTransit(x =>
            {
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

                    cfg.Host(envHostRabbitMqServer);
                });

            });
            services.AddScoped<IOrderCreatedEventPublisher, OrderCreatedEventPublisher>();
            services.AddScoped<ICancelOrderEventPublisher, CancelOrderEventPublisher>();
            return services;
        }
    }
}
