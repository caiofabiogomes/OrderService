namespace OrderService.Application.Abstractions
{
    public interface IOrderNotificationService
    {
        Task NotifyOrderStatusChangedAsync(Guid orderId, string status);
    }
}
