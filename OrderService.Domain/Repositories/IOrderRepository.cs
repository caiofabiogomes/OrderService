using OrderService.Domain.Entities;

namespace OrderService.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid orderId);
        Task<Order?> GetByCustomerIdAndOrderIdAsync(Guid customerId, Guid orderId);
        Task<List<Order>> GetByCustomerIdAsync(Guid customerId, int page, int quantityPerPage);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
    }
}
