using BurgerShopOrdering.api.Controllers;
using BurgerShopOrdering.api.Dtos.Accounts;
using BurgerShopOrdering.api.Dtos.Common;
using BurgerShopOrdering.core.Entities;
using BurgerShopOrdering.core.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.test.API
{
    public class AccountsControllerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock;
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private readonly AccountsController _controller;

        public AccountsControllerTests()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(_userManagerMock.Object, contextAccessor.Object, claimsFactory.Object, null, null, null, null);

            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(roleStore.Object, null, null, null, null);

            _accountServiceMock = new Mock<IAccountService>();

            _controller = new AccountsController(_userManagerMock.Object, _accountServiceMock.Object, _signInManagerMock.Object, _roleManagerMock.Object);
        }
        #region Register
        [Fact]
        public async Task Register_WhenSuccessful_ReturnsOk()
        {
            // Arrange
            var dto = new RegisterUserRequestDto { Email = "test@example.com", Password = "password" };
            _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email)).ReturnsAsync((ApplicationUser)null);
            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
                .ReturnsAsync(IdentityResult.Success);
            _roleManagerMock.Setup(r => r.FindByNameAsync("Client"))
                .ReturnsAsync(new IdentityRole("Client"));

            // Act
            var result = await _controller.Register(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Empty(apiResponse.Errors);
            Assert.Equal("Gebruiker werd geregistreerd", apiResponse.Message);
        }

        [Fact]
        public async Task Register_WhenModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Email", "Required");
            var dto = new RegisterUserRequestDto();

            // Act
            var result = await _controller.Register(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Required", apiResponse.Errors);
            Assert.Equal("Ongeldige invoer.", apiResponse.Message);
        }

        [Fact]
        public async Task Register_WhenUserExists_ReturnsBadRequest()
        {
            // Arrange
            var dto = new RegisterUserRequestDto { Email = "test@example.com", Password = "password" };
            _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email)).ReturnsAsync(new ApplicationUser("test","person", "test@example.com"));

            // Act
            var result = await _controller.Register(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal("Deze gebruiker is reeds geregistreerd.", apiResponse.Message);
        }

        [Fact]
        public async Task Register_WhenAccountServiceReturnsErrors_ReturnsBadRequest()
        {
            // Arrange
            var dto = new RegisterUserRequestDto { Email = "test@example.com", Password = "password" };
            _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email)).ReturnsAsync((ApplicationUser)null);
            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Service error" }));

            // Act
            var result = await _controller.Register(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Service error", apiResponse.Errors);
        }

        [Fact]
        public async Task Register_WhenRoleNotFound_ReturnsServerError()
        {
            // Arrange
            var dto = new RegisterUserRequestDto { Email = "test@example.com", Password = "password" };
            _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email)).ReturnsAsync((ApplicationUser)null);
            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
                .ReturnsAsync(IdentityResult.Success);
            _roleManagerMock.Setup(r => r.FindByNameAsync("Client")).ReturnsAsync((IdentityRole)null);

            // Act
            var result = await _controller.Register(dto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            var apiResponse = Assert.IsType<ApiResponse<object>>(objectResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal("Rol 'Client' bestaat niet in het systeem.", apiResponse.Message);
        }

        
        #endregion

        #region Login
        [Fact]
        public async Task Login_WhenSuccessful_ReturnsOkWithToken()
        {
            // Arrange
            var dto = new LoginUserRequestDto { Email = "test@example.com", Password = "password" };
            var user = new ApplicationUser("test", "person", "test@example.com");
            var token = new JwtSecurityToken();

            _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email)).ReturnsAsync(user);
            _signInManagerMock.Setup(s => s.PasswordSignInAsync(user, dto.Password, false, true))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            _accountServiceMock.Setup(a => a.GenerateTokenAsync(user)).ReturnsAsync(token);

            // Act
            var result = await _controller.Login(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<LoginUserResponseDto>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.NotNull(apiResponse.Data.Token);
            Assert.Empty(apiResponse.Errors);
            Assert.Equal("Login succesvol", apiResponse.Message);
        }

        [Fact]
        public async Task Login_WhenModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Email", "Required");
            var dto = new LoginUserRequestDto();

            // Act
            var result = await _controller.Login(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Required", apiResponse.Errors);
            Assert.Equal("Ongeldige invoer.", apiResponse.Message);
        }

        [Fact]
        public async Task Login_WhenUserNotFound_ReturnsUnauthorized()
        {
            // Arrange
            var dto = new LoginUserRequestDto { Email = "test@example.com", Password = "password" };
            _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email)).ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _controller.Login(dto);

            // Assert
            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(unauthorized.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal("Gebruiker werd niet gevonden.", apiResponse.Message);
        }

        [Fact]
        public async Task Login_WhenLockedOut_ReturnsUnauthorized()
        {
            // Arrange
            var dto = new LoginUserRequestDto { Email = "test@example.com", Password = "wrong" };
            var user = new ApplicationUser("test", "person", "test@example.com");
            _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email)).ReturnsAsync(user);
            _signInManagerMock.Setup(s => s.PasswordSignInAsync(user, dto.Password, false, true))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.LockedOut);

            // Act
            var result = await _controller.Login(dto);

            // Assert
            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(unauthorized.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal("Je account is tijdelijk geblokkeerd wegens te veel mislukte inlogpogingen. Probeer het later opnieuw.", apiResponse.Message);
        }

        [Fact]
        public async Task Login_WhenPasswordIncorrect_ReturnsUnauthorized()
        {
            // Arrange
            var dto = new LoginUserRequestDto { Email = "test@example.com", Password = "wrong" };
            var user = new ApplicationUser("test", "person", "test@example.com");
            _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email)).ReturnsAsync(user);
            _signInManagerMock.Setup(s => s.PasswordSignInAsync(user, dto.Password, false, true))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            // Act
            var result = await _controller.Login(dto);

            // Assert
            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(unauthorized.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal("Ongeldige inloggegevens.", apiResponse.Message);
        }

        #endregion
    }
}
