using BurgerShopApiConsumer.Users;
using BurgerShopApiConsumer.Users.Model;
using BurgerShopOrdering.Core.Models;
using BurgerShopOrdering.Core.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.Core.Services.Web
{
    public class AccountService : IAccountService
    {
        private const string TokenKey = "token";
        private const string UserKey = "CurrentUser";

        private readonly IUserApiService _userApiService;
        private readonly INavigationService _navigationService;

        public AccountService(IUserApiService userApiService, INavigationService navigationService)
        {
            _userApiService = userApiService;
            _navigationService = navigationService;
        }

        public async Task<string> GetTokenAsync()
        {
            return await SecureStorage.Default.GetAsync(TokenKey);
        }
        public async Task<bool> IsAuthenticatedAsync()
        {
            string encodedToken = await GetTokenAsync();

            if (string.IsNullOrEmpty(encodedToken)) { return false; }

            var handler = new JwtSecurityTokenHandler();

            try
            {
                var jsonToken = handler.ReadToken(encodedToken) as JwtSecurityToken;
                return jsonToken != null && jsonToken.ValidTo > DateTime.UtcNow;
            }
            catch
            {
                return false;
            }            
        }
        public async Task<ResultModel> TryLoginAsync(string email, string password)
        {
            var userToLogin = new UserLoginRequestApiModel
            {
                Email = email,
                Password = password
            };

            var response = await _userApiService.LoginAsync(userToLogin);

            if (response.Success && response.Data != null)
            {
                await StoreToken(response.Data.Token);

                await SaveUserToSecureStorage(new User
                {
                    Id = response.Data.User.Id,
                    FirstName = response.Data.User.Firstname,
                    LastName = response.Data.User.Lastname,
                    Email = response.Data.User.Email,
                    IsAdmin = response.Data.User.IsAdmin,
                    IsClient = response.Data.User.IsClient,
                });

                var role = response.Data.User.IsAdmin ? "Admin" : "";
                _navigationService.UpdateTabBarForRole(role);
            }

            return new ResultModel { Success = response.Success, Message = response.Message, Errors = response.Errors};
        }
        public bool Logout()
        {
            bool success = SecureStorage.Default.Remove(TokenKey);
            RemoveUserFromSecureStorage();

            if (success)
            {
                _navigationService.UpdateTabBarForRole("");
            }

            return success;
        }
        public async Task<ResultModel> TryRegisterAsync(string firstname, string lastname, string email, string password, string confirmPassword)
        {
            var userToRegister = new UserRegisterRequestApiModel
            {
                Email = email,
                Password = password,
                ConfirmPassword = confirmPassword,
                FirstName = firstname,
                LastName = lastname,
            };

            var response = await _userApiService.RegisterAsync(userToRegister);

            return new ResultModel { Success = response.Success, Message = response.Message, Errors = response.Errors };
        }
        private async Task StoreToken(string token)
        {
            await SecureStorage.Default.SetAsync(TokenKey, token);
        }
        public async Task<User?> GetLoggedInUserAsync()
        {
            try
            {
                string? userJson = await SecureStorage.Default.GetAsync(UserKey);

                if (string.IsNullOrEmpty(userJson))
                {
                    return null;
                }

                var user = JsonConvert.DeserializeObject<User>(userJson);

                return user;
            }
            catch
            {
                return null;
            }
        }
        private async Task SaveUserToSecureStorage(User user)
        {
            string userJson = JsonConvert.SerializeObject(user);

            await SecureStorage.Default.SetAsync(UserKey, userJson);
        }
        private void RemoveUserFromSecureStorage()
        {
            SecureStorage.Default.Remove(UserKey);
        }
    }
}
