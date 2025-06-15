using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Abstractions;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;

namespace OrderService.Infraestructure.Persistence.Repositories
{
    public class OrderRepository(OrderServiceDBContext context) : IOrderRepository
    {
        private readonly OrderServiceDBContext _context = context;

        public async Task<Order?> GetByIdAsync(Guid orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            return order;
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Order?> GetByCustomerIdAndOrderIdAsync(Guid customerId, Guid orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.CustomerId == customerId && o.Id == orderId); 

            return order;
        }

        public async Task<PagedResult<Order>> GetByCustomerIdAsync(Guid customerId, int page, int quantityPerPage)
        {
            var totalOrders = await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .CountAsync();

            var totalPages = (int)Math.Ceiling(totalOrders / (double)quantityPerPage);

            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.CustomerId == customerId)
                .Skip((page - 1) * quantityPerPage)
                .Take(quantityPerPage)
                .ToListAsync();

            return new PagedResult<Order>
            {
                Items = orders,
                TotalPages = totalPages,
                CurrentPage = page
            };
        }

    }
}
