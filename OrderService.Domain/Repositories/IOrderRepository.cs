﻿using OrderService.Domain.Abstractions;
using OrderService.Domain.Entities;

namespace OrderService.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid orderId);
        Task<Order?> GetByCustomerIdAndOrderIdAsync(Guid customerId, Guid orderId);
        Task<PagedResult<Order>> GetByCustomerIdAsync(Guid customerId, int page, int quantityPerPage);
        Task AddWithoutSaveChangesAsync(Order order);
        Task SaveChangesAsync();
    }
}
