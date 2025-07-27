using BurgerShopApiConsumer.Common;
using BurgerShopApiConsumer.Users;
using BurgerShopApiConsumer.Users.Model;
using BurgerShopOrdering.Core.Models;
using BurgerShopOrdering.Core.Services.Interfaces;
using BurgerShopOrdering.Core.Services.Web;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopTests.test
{
    public class AccountServiceTests
    {
        private readonly Mock<IUserApiService> _userApiServiceMock = new();
        private readonly Mock<INavigationService> _navigationServiceMock = new();
        private readonly AccountService _accountService;

        public AccountServiceTests()
        {
            _accountService = new AccountService(_userApiServiceMock.Object, _navigationServiceMock.Object);
        }

        [Fact]
        public async Task TryLoginAsync_WhenLoginFails_ReturnsFailure()
        {
            //Arrange
            _userApiServiceMock.Setup(x => x.LoginAsync(It.IsAny<UserLoginRequestApiModel>()))
                .ReturnsAsync(ApiResponse<UserLoginResponseApiModel>.FailureResponse("Invalid login"));

            //Act
            var result = await _accountService.TryLoginAsync("fail@test.com", "wrongpass");

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid login", result.Message);
            _navigationServiceMock.Verify(x => x.UpdateTabBarForRole(It.IsAny<string>()), Times.Never);
        }
        [Fact]
        public async Task TryRegisterAsync_WhenApiReturnsSuccess_ReturnsSuccesResult()
        {
            //Arrange
            _userApiServiceMock.Setup(x => x.RegisterAsync(It.IsAny<UserRegisterRequestApiModel>()))
                .ReturnsAsync(ApiResponse<object>.SuccessResponse(null, "Registered"));

            //Act
            var result = await _accountService.TryRegisterAsync("John", "Doe", "test@test.com", "pass", "pass");

            //Assert
            Assert.True(result.Success);
            Assert.Equal("Registered", result.Message);
        }
        [Fact]
        public async Task TryRegisterAsync_WhenApiFails_ReturnsFailResult()
        {
            //Arrange
            _userApiServiceMock.Setup(x => x.RegisterAsync(It.IsAny<UserRegisterRequestApiModel>()))
                .ReturnsAsync(ApiResponse<object>.SuccessResponse(null, "Error"));

            //Act
            var result = await _accountService.TryRegisterAsync("John", "Doe", "test@test.com", "pass", "pass");

            //Assert
            Assert.True(result.Success);
            Assert.Equal("Error", result.Message);
        }
    }
}
