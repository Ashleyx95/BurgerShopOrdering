using BurgerShopApiConsumer.Categories.Models;
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

namespace BurgerShopApiConsumer.Categories
{
    public class CategoryApiService : ICategoryApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _burgerShopApiClient;

        public CategoryApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _burgerShopApiClient = _httpClientFactory.CreateClient("burgerShopApiClient");
            _burgerShopApiClient.BaseAddress = new Uri(ApiRoutes.Categories);
        }

        public async Task<ApiResponse<CategoryResponseApiModel[]>> GetCategoriesAsync()
        {
            try
            {
                var response = await _burgerShopApiClient.GetAsync("");

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return ApiResponse<CategoryResponseApiModel[]>.FailureResponse("Er is een serverfout opgetreden. Probeer het later opnieuw.");
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<CategoryResponseApiModel[]>>();

                return result ?? ApiResponse<CategoryResponseApiModel[]>.FailureResponse("Fout bij ophalen categorieën.");
            }
            catch (HttpRequestException)
            {
                return ApiResponse<CategoryResponseApiModel[]>.FailureResponse("Kan geen verbinding maken met de server.");
            }
            catch (Exception ex)
            {
                return ApiResponse<CategoryResponseApiModel[]>.FailureResponse("Er is een onverwachte fout opgetreden.", new[] { ex.Message });
            }   
        }

        public async Task<ApiResponse<CategoryResponseApiModel>> GetCategoryByIdAsync(Guid id)
        {
            try
            {
                var response = await _burgerShopApiClient.GetAsync($"{id}");

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return ApiResponse<CategoryResponseApiModel>.FailureResponse("Er is een serverfout opgetreden. Probeer het later opnieuw.");
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<CategoryResponseApiModel>>();

                return result ?? ApiResponse<CategoryResponseApiModel>.FailureResponse("Fout bij ophalen categorie.");
            }
            catch (HttpRequestException)
            {
                return ApiResponse<CategoryResponseApiModel>.FailureResponse("Kan geen verbinding maken met de server.");
            }
            catch (Exception ex)
            {
                return ApiResponse<CategoryResponseApiModel>.FailureResponse("Er is een onverwachte fout opgetreden.", new[] { ex.Message });
            }

        }

        public async Task<ApiResponse<object>> CreateCategoryAsync(CategoryCreateRequestApiModel categoryToCreate, string token)
        {
            try
            {
                _burgerShopApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _burgerShopApiClient.PostAsJsonAsync("", categoryToCreate);

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return ApiResponse<object>.FailureResponse("Er is een serverfout opgetreden. Probeer het later opnieuw.");
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();

                return result ?? ApiResponse<object>.FailureResponse("Fout bij aanmaken categorie.");
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

        public async Task<ApiResponse<object>> UpdateCategoryAsync(CategoryUpdateRequestApiModel categoryToUpdate, string token)
        {
            try
            {
                _burgerShopApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _burgerShopApiClient.PutAsJsonAsync("", categoryToUpdate);

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return ApiResponse<object>.FailureResponse("Er is een serverfout opgetreden. Probeer het later opnieuw.");
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();

                return result ?? ApiResponse<object>.FailureResponse("Fout bij updaten categorie.");
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

        public async Task<ApiResponse<object>> DeleteCategoryAsync(Guid id, string token)
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

                return result ?? ApiResponse<object>.FailureResponse("Fout bij verwijderen categorie.");
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
