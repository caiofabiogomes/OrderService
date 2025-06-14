using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace OrderService.Infraestructure.Persistence
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new OrderServiceDBContext(serviceProvider.GetRequiredService<DbContextOptions<OrderServiceDBContext>>()))
            {
                context.Database.Migrate();
            }
        }
    }
}
