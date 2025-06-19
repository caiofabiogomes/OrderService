using Microsoft.IdentityModel.Tokens;
using OrderService.Application.Abstractions;
using OrderService.Application.Commands.PlaceOrder;
using OrderService.Application.ExternalServices;
using OrderService.Application.ViewModels;
using System.Net.Http.Json;

namespace OrderService.Infrastructure.ExternalServices
{
    public class GetItemsOrderService : IGetItemsOrderService
    {
        private readonly HttpClient _httpClient;

        public GetItemsOrderService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<Result<List<GetItemsViewModel>>> GetItemsAsync(List<Guid> itemsIds)
        {
            if (itemsIds == null || !itemsIds.Any())
                return Result<List<GetItemsViewModel>>.Failure("Order ID cannot be empty.");

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/v1/products/getProductsList", itemsIds);

                if (!response.IsSuccessStatusCode)
                    return Result<List<GetItemsViewModel>>.Failure("Failed to retrieve items from the external service.");

                if (response.Content == null)
                    return Result<List<GetItemsViewModel>>.Success(new List<GetItemsViewModel>());

                var result = await response.Content.ReadFromJsonAsync<List<GetItemsViewModel>>();

                return Result<List<GetItemsViewModel>>.Success(result ?? new List<GetItemsViewModel>(), "Items retrieved successfully.");
            }
            catch
            {
                return Result<List<GetItemsViewModel>>.Failure("Failed to retrieve items from the external service.");
            }
        }
    }
}
