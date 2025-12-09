using System.Text.Json;
using  AlphaProject.Shared.Dtos;    

namespace AlphaProject.CRM
{
    public class OrdersClient
    {
        private readonly HttpClient _httpClient;

        public OrdersClient(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ApiWithUserToken");
        }

        public async Task<List<OrderDto>> GetOrdersAsync()
        {
            var response = await _httpClient.GetAsync("api/orders");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<OrderDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }
    }

}
