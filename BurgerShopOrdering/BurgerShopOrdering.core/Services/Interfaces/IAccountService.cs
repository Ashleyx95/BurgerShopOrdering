using BurgerShopOrdering.core.Entities;
using BurgerShopOrdering.core.Services.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.core.Services.Interfaces
{
    public interface IAccountService
    {
        Task<JwtSecurityToken> GenerateTokenAsync(ApplicationUser applicationUser);
    }
}
