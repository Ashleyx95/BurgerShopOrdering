using BurgerShopApiConsumer.Common;
using BurgerShopApiConsumer.Orders.Model;
using BurgerShopApiConsumer.Products.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopApiConsumer.Orders
{
    public class OrderApiService : IOrderApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _burgerShopApiClient;

        public OrderApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _burgerShopApiClient = _httpClientFactory.CreateClient("burgerShopApiClient");
            _burgerShopApiClient.BaseAddress = new Uri(ApiRoutes.Orders);
        }

        public async Task<ApiResponse<OrderResponseApiModel[]>> GetOrdersAsync(string token)
        {
            try
            {
                _burgerShopApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _burgerShopApiClient.GetAsync("");

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return ApiResponse<OrderResponseApiModel[]>.FailureResponse("Er is een serverfout opgetreden. Probeer het later opnieuw.");
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<OrderResponseApiModel[]>>();

                return result ?? ApiResponse<OrderResponseApiModel[]>.FailureResponse("Fout bij ophalen bestellingen.");
            }
            catch (HttpRequestException)
            {
                return ApiResponse<OrderResponseApiModel[]>.FailureResponse("Kan geen verbinding maken met de server.");
            }
            catch (Exception ex)
            {
                return ApiResponse<OrderResponseApiModel[]>.FailureResponse("Er is een onverwachte fout opgetreden.", new[] { ex.Message });
            }

        }

        public async Task<ApiResponse<OrderResponseApiModel>> GetOrderByIdAsync(Guid id, string token)
        {
            try
            {
                _burgerShopApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _burgerShopApiClient.GetAsync($"{id}");

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return ApiResponse<OrderResponseApiModel>.FailureResponse("Er is een serverfout opgetreden. Probeer het later opnieuw.");
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<OrderResponseApiModel>>();

                return result ?? ApiResponse<OrderResponseApiModel>.FailureResponse("Fout bij ophalen bestelling.");
            }
            catch (HttpRequestException)
            {
                return ApiResponse<OrderResponseApiModel>.FailureResponse("Kan geen verbinding maken met de server.");
            }
            catch (Exception ex)
            {
                return ApiResponse<OrderResponseApiModel>.FailureResponse("Er is een onverwachte fout opgetreden.", new[] { ex.Message });
            }
        }

        public async Task<ApiResponse<OrderResponseApiModel[]>> GetOrdersByStatusAsync(string status, string token)
        {
            try
            {
                _burgerShopApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _burgerShopApiClient.GetAsync($"{status}");

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return ApiResponse<OrderResponseApiModel[]>.FailureResponse("Er is een serverfout opgetreden. Probeer het later opnieuw.");
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<OrderResponseApiModel[]>>();

                return result ?? ApiResponse<OrderResponseApiModel[]>.FailureResponse("Fout bij ophalen bestellingen.");
            }
            catch (HttpRequestException)
            {
                return ApiResponse<OrderResponseApiModel[]>.FailureResponse("Kan geen verbinding maken met de server.");
            }
            catch (Exception ex)
            {
                return ApiResponse<OrderResponseApiModel[]>.FailureResponse("Er is een onverwachte fout opgetreden.", new[] { ex.Message });
            }
        }

        public async Task<ApiResponse<object>> CreateOrderAsync(OrderCreateRequestApiModel orderToCreate, string token)
        {
            try
            {
                _burgerShopApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _burgerShopApiClient.PostAsJsonAsync("", orderToCreate);

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return ApiResponse<object>.FailureResponse("Er is een serverfout opgetreden. Probeer het later opnieuw.");
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();

                return result ?? ApiResponse<object>.FailureResponse("Fout bij aanmaken bestelling.");
            }
            catch (HttpRequestException)
            {
                return ApiResponse<object>.FailureResponse("Kan geen verbinding maken met de server.");
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.FailureResponse("Er is een onverwachte fout opgetreden.", new[] { ex.Message });
            }
        }

        public async Task<ApiResponse<object>> UpdateOrderAsync(OrderUpdateRequestApiModel orderToUpdate, string token)
        {
            try
            {
                _burgerShopApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _burgerShopApiClient.PutAsJsonAsync("", orderToUpdate);

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return ApiResponse<object>.FailureResponse("Er is een serverfout opgetreden. Probeer het later opnieuw.");
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();

                return result ?? ApiResponse<object>.FailureResponse("Fout bij updaten bestelling.");
            }
            catch (HttpRequestException)
            {
                return ApiResponse<object>.FailureResponse("Kan geen verbinding maken met de server.");
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.FailureResponse("Er is een onverwachte fout opgetreden.", new[] { ex.Message });
            }

        }

        public async Task<ApiResponse<object>> DeleteOrderAsync(Guid id, string token)
        {
            try
            {
                _burgerShopApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _burgerShopApiClient.DeleteAsync($"{id}");

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return ApiResponse<object>.FailureResponse("Er is een serverfout opgetreden. Probeer het later opnieuw.");
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();

                return result ?? ApiResponse<object>.FailureResponse("Fout bij verwijderen bestelling.");
            }
            catch (HttpRequestException)
            {
                return ApiResponse<object>.FailureResponse("Kan geen verbinding maken met de server.");
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.FailureResponse("Er is een onverwachte fout opgetreden.", new[] { ex.Message });
            }
        }
    }
}
