using BurgerShopApiConsumer.Common;
using BurgerShopApiConsumer.Users.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopApiConsumer.Users
{
    public class UserApiService : IUserApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _burgerShopApiClient;

        public UserApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _burgerShopApiClient = _httpClientFactory.CreateClient("burgerShopApiClient");
            _burgerShopApiClient.BaseAddress = new Uri(ApiRoutes.Accounts);
        }

        public async Task<ApiResponse<UserLoginResponseApiModel>> LoginAsync(UserLoginRequestApiModel userToLogin)
        {
            try
            {
                var response = await _burgerShopApiClient.PostAsJsonAsync("Login", userToLogin);

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return ApiResponse<UserLoginResponseApiModel>.FailureResponse("Er is een serverfout opgetreden. Probeer het later opnieuw.");
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<UserLoginResponseApiModel>>();

                return result ?? ApiResponse<UserLoginResponseApiModel>.FailureResponse("Fout bij inloggen.");
            }
            catch (HttpRequestException)
            {
                return ApiResponse<UserLoginResponseApiModel>.FailureResponse("Kan geen verbinding maken met de server.");
            }
            catch (Exception ex)
            {
                return ApiResponse<UserLoginResponseApiModel>.FailureResponse("Er is een onverwachte fout opgetreden.", new[] { ex.Message });
            }
        }

        public async Task<ApiResponse<object>> RegisterAsync(UserRegisterRequestApiModel userToRegister)
        {
            try
            {
                var response = await _burgerShopApiClient.PostAsJsonAsync("Register", userToRegister);

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return ApiResponse<object>.FailureResponse("Er is een serverfout opgetreden. Probeer het later opnieuw.");
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();

                return result ?? ApiResponse<object>.FailureResponse("Fout bij registreren.");
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
