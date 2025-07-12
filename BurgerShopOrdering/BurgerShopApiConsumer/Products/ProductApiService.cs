using BurgerShopApiConsumer.Common;
using BurgerShopApiConsumer.Products.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopApiConsumer.Products
{
    public class ProductApiService : IProductApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _burgerShopApiClient;

        public ProductApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _burgerShopApiClient = _httpClientFactory.CreateClient("burgerShopApiClient");
            _burgerShopApiClient.BaseAddress = new Uri(ApiRoutes.Products);
        }

        public async Task<ApiResponse<ProductResponseApiModel[]>> GetProductsAsync()
        {
            try
            {
                var response = await _burgerShopApiClient.GetAsync("");

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return ApiResponse<ProductResponseApiModel[]>.FailureResponse("Er is een serverfout opgetreden. Probeer het later opnieuw.");
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductResponseApiModel[]>>();

                return result ?? ApiResponse<ProductResponseApiModel[]>.FailureResponse("Fout bij ophalen producten.");
            }
            catch (HttpRequestException)
            {
                return ApiResponse<ProductResponseApiModel[]>.FailureResponse("Kan geen verbinding maken met de server.");
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductResponseApiModel[]>.FailureResponse("Er is een onverwachte fout opgetreden.", new[] { ex.Message }) ;
            }
        }

        public async Task<ApiResponse<ProductResponseApiModel>> GetProductByIdAsync(Guid id)
        {
            try
            {
                var response = await _burgerShopApiClient.GetAsync($"{id}");

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return ApiResponse<ProductResponseApiModel>.FailureResponse("Er is een serverfout opgetreden. Probeer het later opnieuw.");
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductResponseApiModel>>();

                return result ?? ApiResponse<ProductResponseApiModel>.FailureResponse("Fout bij ophalen product.");
            }
            catch (HttpRequestException)
            {
                return ApiResponse<ProductResponseApiModel>.FailureResponse("Kan geen verbinding maken met de server.");
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductResponseApiModel>.FailureResponse("Er is een onverwachte fout opgetreden.", new[] { ex.Message });
            }
            
        }

        public async Task<ApiResponse<object>> CreateProductAsync(ProductCreateRequestApiModel productToCreate, string token)
        {
            try
            {
                _burgerShopApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _burgerShopApiClient.PostAsJsonAsync("", productToCreate);

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return ApiResponse<object>.FailureResponse("Er is een serverfout opgetreden. Probeer het later opnieuw.");
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();

                return result ?? ApiResponse<object>.FailureResponse("Fout bij aanmaken product.");
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

        public async Task<ApiResponse<object>> UpdateProductAsync(ProductUpdateRequestApiModel productToUpdate, string token)
        {
            try
            {
                _burgerShopApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _burgerShopApiClient.PutAsJsonAsync("", productToUpdate);

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return ApiResponse<object>.FailureResponse("Er is een serverfout opgetreden. Probeer het later opnieuw.");
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();

                return result ?? ApiResponse<object>.FailureResponse("Fout bij updaten product.");
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

        public async Task<ApiResponse<object>> DeleteProductAsync(Guid id, string token)
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

                return result ?? ApiResponse<object>.FailureResponse("Fout bij verwijderen product.");
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
