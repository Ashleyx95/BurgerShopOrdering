using BurgerShopOrdering.core.Entities;
using BurgerShopOrdering.core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.test.Core.Services
{
    public class AccountServiceTests
    {
        private UserManager<ApplicationUser> GetMockUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null).Object;
        }

        private IConfiguration GetMockConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "JWTConfiguration:TokenExpirationDays", "7" },
                { "JWTConfiguration:SigningKey", "ThisIsASuperSecretKey123!" },
                { "JWTConfiguration:Issuer", "BurgerShopAPI" },
                { "JWTConfiguration:Audience", "BurgerShopClients" }
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        #region GenerateTokenAsync
        [Fact]
        public async Task GenerateTokenAsync_ReturnsValidJwtToken()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null
            );

            var user = new ApplicationUser("test", "person", "testperson@hotmail.com");

            userManagerMock.Setup(x => x.GetClaimsAsync(user)).ReturnsAsync(new List<Claim>());
            userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Admin", "Client" });

            var configuration = GetMockConfiguration();

            var service = new AccountService(userManagerMock.Object, configuration);

            // Act
            var token = await service.GenerateTokenAsync(user);

            // Assert
            Assert.NotNull(token);
            Assert.IsType<JwtSecurityToken>(token);
            Assert.Contains(token.Claims, c => c.Type == ClaimTypes.NameIdentifier && c.Value == user.Id);
            Assert.Contains(token.Claims, c => c.Type == ClaimTypes.Name && c.Value == user.UserName);
            Assert.Contains(token.Claims, c => c.Type == ClaimTypes.Role && c.Value == "Admin");
            Assert.Contains(token.Claims, c => c.Type == ClaimTypes.Role && c.Value == "Client");
        }
        #endregion
    }
}
