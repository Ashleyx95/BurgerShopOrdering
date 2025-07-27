using BurgerShopApiConsumer.Common;
using BurgerShopApiConsumer.Users.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopApiConsumer.Users
{
    public interface IUserApiService
    {
        Task<ApiResponse<UserLoginResponseApiModel>> LoginAsync(UserLoginRequestApiModel userToLogin);
        Task<ApiResponse<object>> RegisterAsync(UserRegisterRequestApiModel userToRegister);
    }
}
