using BurgerShopOrdering.core.Data;
using BurgerShopOrdering.core.Entities;
using BurgerShopOrdering.core.Services.Interfaces;
using BurgerShopOrdering.core.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.core.Services
{
    public class AccountService(UserManager<ApplicationUser> userManager, IConfiguration configuration, BurgerShopDbContext burgerDbContext) : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;
        private BurgerShopDbContext _burgerDbContext = burgerDbContext;

        public async Task<JwtSecurityToken> GenerateTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>();

            var userClaims = await _userManager.GetClaimsAsync(user);

            claims.AddRange(userClaims);
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserName));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));

            var roleClaims = await _userManager.GetRolesAsync(user);

            foreach (var roleClaim in roleClaims)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleClaim));
            }

            var expirationDays = _configuration.GetValue<int>("JWTConfiguration:TokenExpirationDays");
            var signingKey = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWTConfiguration:SigningKey"));
            var token = new JwtSecurityToken
            (
                issuer: _configuration.GetValue<string>("JWTConfiguration:Issuer"),
                audience: _configuration.GetValue<string>("JWTConfiguration:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(expirationDays)),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256)
            );

            return token;
        }

        public async Task<ResultModel<ApplicationUser>> GetUserByEmail(string email)
        {
            var user = await burgerDbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.NormalizedEmail == email.ToUpper());

            var resultModel = new ResultModel<ApplicationUser>();

            if (user is null)
            {
                resultModel.Errors.Add("Deze gebruiker is nog niet geregistreerd");
            }
            else
            {
                resultModel.Data = user;
            }

            return resultModel;
        }
    }
}
