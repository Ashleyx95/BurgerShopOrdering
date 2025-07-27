using BurgerShopOrdering.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.Core.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> IsAuthenticatedAsync();
        Task<ResultModel> TryLoginAsync(string email, string password);
        Task<ResultModel> TryRegisterAsync(string firstname, string lastname, string email, string password, string confirmPassword);
        Task<string> GetTokenAsync();
        Task<User?> GetLoggedInUserAsync();
        bool Logout();
    }
}
