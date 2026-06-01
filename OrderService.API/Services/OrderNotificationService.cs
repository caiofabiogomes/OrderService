using Microsoft.AspNetCore.SignalR;
using OrderService.API.Hubs;
using OrderService.Application.Abstractions;

namespace OrderService.API.Services
{
    public class OrderNotificationService(IHubContext<OrderHub> hubContext) : IOrderNotificationService
    {
        private readonly IHubContext<OrderHub> _hubContext = hubContext;

        public async Task NotifyOrderStatusChangedAsync(Guid orderId, string status)
        {
            await _hubContext.Clients.All.SendAsync("UpdateOrderStatus", orderId, status);
        }
    }
}
