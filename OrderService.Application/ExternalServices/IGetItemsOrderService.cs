using OrderService.Application.Abstractions;
using OrderService.Application.ViewModels;

namespace OrderService.Application.ExternalServices
{
    public interface IGetItemsOrderService
    {
        Task<Result<List<GetItemsViewModel>>> GetItemsAsync(List<Guid> itemsIds);
    }
}
