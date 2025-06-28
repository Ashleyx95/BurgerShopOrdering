using BurgerShopOrdering.api.Controllers;
using BurgerShopOrdering.api.Dtos.Common;
using BurgerShopOrdering.api.Dtos.Orders;
using BurgerShopOrdering.core.Entities;
using BurgerShopOrdering.core.Services.Interfaces;
using BurgerShopOrdering.core.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.test.API
{
    public class OrdersControllerTests
    {
        private readonly Mock<IOrderService<Order>> _orderServiceMock;
        private readonly Mock<ICrudService<OrderItem>> _orderItemServiceMock;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            _orderServiceMock = new Mock<IOrderService<Order>>();
            _orderItemServiceMock = new Mock<ICrudService<OrderItem>>();
            _userManagerMock = MockUserManager<ApplicationUser>();

            _controller = new OrdersController(_orderServiceMock.Object, _orderItemServiceMock.Object, _userManagerMock.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
                new Claim(ClaimTypes.Role, "Client")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        private static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            return new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        private void SetUserAsAdmin()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "admin-id"),
                new Claim(ClaimTypes.Role, "Admin")
            }, "mock"));
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        }

        private void SetUserAsClient(string userId)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "mock"));
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        }

        #region Get
        [Fact]
        public async Task Get_WhenUserIsClient_ReturnsOkWithUserOrders()
        {
            // Arrange
            var user = new ApplicationUser("test", "person", "testperson@hotmail.com");

            SetUserAsClient(user.Id);

            var order = new Order(user.Id, "Order1", 90, 2) { ApplicationUser = user};
            var product1 = new Product("Burger", 50);
            var product2 = new Product("Sauce", 40);

            order.OrderItems = new List<OrderItem> { new OrderItem(order.Id, product1.Id, 1, 50) { Product = product1}, new OrderItem(order.Id, product2.Id, 1, 40) { Product = product2} };

            var orders = new List<Order>
            {
                order
            };

            _orderServiceMock.Setup(s => s.GetOrdersByUserAsync(user.Id))
                .ReturnsAsync(new ResultModel<IEnumerable<Order>> { Data = orders });

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<List<OrderResponseDto>>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Single(apiResponse.Data);
            Assert.Empty(apiResponse.Errors);
            Assert.Equal("Order1", apiResponse.Data[0].Name);
            Assert.Equal("Bestellingen opgehaald.", apiResponse.Message);
        }

        [Fact]
        public async Task Get_WhenUserIsAdmin_ReturnsOkWithAllOrders()
        {
            // Arrange
            SetUserAsAdmin();

            var client = new ApplicationUser("test", "person", "testperson@hotmail.com");

            var order = new Order(client.Id, "Order1", 90, 2) { ApplicationUser = client };
            var product1 = new Product("Burger", 50);
            var product2 = new Product("Sauce", 40);

            order.OrderItems = new List<OrderItem> { new OrderItem(order.Id, product1.Id, 1, 50) { Product = product1 }, new OrderItem(order.Id, product2.Id, 1, 40) { Product = product2 } };

            var orders = new List<Order>
            {
                order
            };

            _orderServiceMock.Setup(s => s.GetAllAsync())
                .ReturnsAsync(new ResultModel<IEnumerable<Order>> { Data = orders });

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<List<OrderResponseDto>>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Single(apiResponse.Data);
            Assert.Empty(apiResponse.Errors);
            Assert.Equal("Order1", apiResponse.Data[0].Name);
            Assert.Equal("Bestellingen opgehaald.", apiResponse.Message);
        }

        [Fact]
        public async Task Get_WhenOrderServiceReturnsErrors_ReturnsBadRequest()
        {
            // Arrange
            _orderServiceMock.Setup(s => s.GetOrdersByUserAsync("test-user-id"))
                .ReturnsAsync(new ResultModel<IEnumerable<Order>> { Errors = new List<string> { "Service error" } });

            // Act
            var result = await _controller.Get();

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Service error", apiResponse.Errors);
            Assert.Equal("Bestellingen konden niet worden opgehaald.", apiResponse.Message);
        }

        [Fact]
        public async Task GetById_WhenOrderExistsAndUserAuthorized_ReturnsOk()
        {
            // Arrange
            var user = new ApplicationUser("test", "person", "testperson@hotmail.com");

            SetUserAsClient(user.Id);

            var order = new Order(user.Id, "Order1", 90, 2) { ApplicationUser = user };
            var product1 = new Product("Burger", 50);
            var product2 = new Product("Sauce", 40);

            order.OrderItems = new List<OrderItem> { new OrderItem(order.Id, product1.Id, 1, 50) { Product = product1 }, new OrderItem(order.Id, product2.Id, 1, 40) { Product = product2 } };

            var orders = new List<Order>
            {
                order
            };

            _orderServiceMock.Setup(s => s.GetByIdAsync(order.Id))
                .ReturnsAsync(new ResultModel<Order> { Data = order });

            // Act
            var result = await _controller.Get(order.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<OrderResponseDto>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(order.Id, apiResponse.Data.Id);
            Assert.Equal("test person", apiResponse.Data.NameUser);
            Assert.Equal("Bestelling werd opgehaald.", apiResponse.Message);
        }

        [Fact]
        public async Task GetById_WhenOrderNotFound_ReturnsNotFound()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            _orderServiceMock.Setup(s => s.GetByIdAsync(orderId))
                .ReturnsAsync(new ResultModel<Order> { Data = null, Errors = new List<string> { "Not found" } });

            // Act
            var result = await _controller.Get(orderId);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(notFound.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Not found", apiResponse.Errors);
            Assert.Equal("Bestelling niet gevonden.", apiResponse.Message);
        }

        [Fact]
        public async Task GetById_WhenUserNotAuthorized_ReturnsForbid()
        {
            // Arrange
            var user = new ApplicationUser("test", "person", "testperson@hotmail.com");

            var order = new Order(user.Id, "Order1", 100, 2);

            _orderServiceMock.Setup(s => s.GetByIdAsync(order.Id))
                .ReturnsAsync(new ResultModel<Order> { Data = order });

            // Act
            var result = await _controller.Get(order.Id);

            // Assert
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task GetByStatus_WhenUserIsAdmin_ReturnsOk()
        {
            // Arrange
            SetUserAsAdmin();

            var client = new ApplicationUser("test", "person", "testperson@hotmail.com");

            var order = new Order(client.Id, "Order1", 90, 2) { ApplicationUser = client, Status = OrderStatus.Afgehaald };
            var product1 = new Product("Burger", 50);
            var product2 = new Product("Sauce", 40);

            order.OrderItems = new List<OrderItem> { new OrderItem(order.Id, product1.Id, 1, 50) { Product = product1 }, new OrderItem(order.Id, product2.Id, 1, 40) { Product = product2 } };

            var orders = new List<Order>
            {
                order
            };

            _orderServiceMock.Setup(s => s.GetByStatusAsync(OrderStatus.Afgehaald)).ReturnsAsync(new ResultModel<IEnumerable<Order>> { Data = orders });

            // Act
            var result = await _controller.Get(OrderStatus.Afgehaald);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<List<OrderResponseDto>>>(ok.Value);
            Assert.True(apiResponse.Success);
            Assert.Single(apiResponse.Data);
            Assert.Empty(apiResponse.Errors);
            Assert.Equal("Bestellingen opgehaald.", apiResponse.Message);
        }

        [Fact]
        public async Task GetByStatus_WhenUserIsClient_ReturnsOk()
        {
            // Arrange
            var user = new ApplicationUser("test", "person", "testperson@hotmail.com");

            SetUserAsClient(user.Id);

            var order = new Order(user.Id, "Order1", 90, 2) { ApplicationUser = user, Status = OrderStatus.Afgehaald };
            var product1 = new Product("Burger", 50);
            var product2 = new Product("Sauce", 40);

            order.OrderItems = new List<OrderItem> { new OrderItem(order.Id, product1.Id, 1, 50) { Product = product1 }, new OrderItem(order.Id, product2.Id, 1, 40) { Product = product2 } };

            var orders = new List<Order>
            {
                order
            };

            _orderServiceMock.Setup(s => s.GetOrdersByUserAndStatusAsync(user.Id, OrderStatus.Afgehaald)).ReturnsAsync(new ResultModel<IEnumerable<Order>> { Data = orders });

            // Act
            var result = await _controller.Get(OrderStatus.Afgehaald);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<List<OrderResponseDto>>>(ok.Value);
            Assert.True(apiResponse.Success);
            Assert.Single(apiResponse.Data);
            Assert.Empty(apiResponse.Errors);
            Assert.Equal("Bestellingen opgehaald.", apiResponse.Message);
        }

        [Fact]
        public async Task GetByStatus_AsClientWithoutUserId_ReturnsUnauthorized()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() } };

            // Act
            var result = await _controller.Get(OrderStatus.Afgehaald);

            // Assert
            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(unauthorized.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal("Gebruikers id kon niet worden bepaald.", apiResponse.Message);
        }

        [Fact]
        public async Task GetByStatus_WhenOrderServiceReturnsErrors_ReturnsBadRequest()
        {
            // Arrange
            SetUserAsAdmin();
            _orderServiceMock.Setup(s => s.GetByStatusAsync(OrderStatus.Afgehaald)).ReturnsAsync(new ResultModel<IEnumerable<Order>> { Data = null, Errors = new List<string> { "Service error" } });

            // Act
            var result = await _controller.Get(OrderStatus.Afgehaald);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Service error", apiResponse.Errors);
            Assert.Equal("Bestellingen konden niet worden opgehaald.", apiResponse.Message);
        }

        #endregion

        #region Add
        [Fact]
        public async Task Add_WhenOrderAdded_ReturnsCreated()
        {
            // Arrange
            var dto = new OrderCreateRequestDto
            {
                TotalPrice = 100,
                TotalQuantity = 2,
                OrderItems = new List<OrderItemCreateRequestDto>
                {
                    new OrderItemCreateRequestDto { ProductId = Guid.NewGuid(), Price = 50, Quantity = 1 }
                }
            };

            var user = new ApplicationUser("test", "person", "testperson@hotmail.com");

            SetUserAsClient(user.Id);

            _userManagerMock.Setup(um => um.FindByIdAsync(user.Id))
                .ReturnsAsync(user);

            _orderServiceMock.Setup(s => s.AddAsync(It.IsAny<Order>()))
                .ReturnsAsync(new ResultModel<Order> { });

            // Act
            var result = await _controller.Add(dto);

            // Assert
            var createdAtAction = Assert.IsType<CreatedAtActionResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(createdAtAction.Value);
            Assert.True(apiResponse.Success);
            Assert.Empty(apiResponse.Errors);
            Assert.Equal("Bestelling is toegevoegd.", apiResponse.Message);
        }

        [Fact]
        public async Task Add_WhenModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("OrderItems", "Required");

            var dto = new OrderCreateRequestDto();

            // Act
            var result = await _controller.Add(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Required", apiResponse.Errors);
            Assert.Equal("Ongeldige invoer.", apiResponse.Message);
        }

        [Fact]
        public async Task Add_WhenUserNotFound_ReturnsUnauthorized()
        {
            // Arrange
            var dto = new OrderCreateRequestDto
            {
                TotalPrice = 100,
                TotalQuantity = 2,
                OrderItems = new List<OrderItemCreateRequestDto>
                {
                    new OrderItemCreateRequestDto { ProductId = Guid.NewGuid(), Price = 50, Quantity = 1 }
                }
            };

            _userManagerMock.Setup(um => um.FindByIdAsync("test-user-id"))
                .ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _controller.Add(dto);

            // Assert
            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(unauthorized.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal("Gebruiker niet gevonden.", apiResponse.Message);
        }

        [Fact]
        public async Task Add_WhenOrderServiceReturnsErrors_ReturnsBadRequest()
        {
            // Arrange
            var dto = new OrderCreateRequestDto
            {
                TotalPrice = 100,
                TotalQuantity = 2,
                OrderItems = new List<OrderItemCreateRequestDto>
                {
                    new OrderItemCreateRequestDto { ProductId = Guid.NewGuid(), Price = 50, Quantity = 1 }
                }
            };

            var user = new ApplicationUser("test", "person", "testperson@hotmail.com");

            SetUserAsClient(user.Id);

            _userManagerMock.Setup(um => um.FindByIdAsync(user.Id))
                .ReturnsAsync(user);

            _orderServiceMock.Setup(s => s.AddAsync(It.IsAny<Order>()))
                .ReturnsAsync(new ResultModel<Order> { Errors = new List<string> { "Service error" } });

            // Act
            var result = await _controller.Add(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Service error", apiResponse.Errors);
            Assert.Equal("Bestelling kon niet worden toegevoegd.", apiResponse.Message);
        }

        #endregion

        #region Update

        [Fact]
        public async Task Update_WhenUpdateSucceeds_ReturnsOk()
        {
            // Arrange
            var order = new Order("UserId", "Order1", 50, 2) { Status = OrderStatus.Besteld };

            var dto = new OrderUpdateRequestDto { Id = order.Id, Status = OrderStatus.Afgehaald };

            _orderServiceMock.Setup(s => s.GetByIdAsync(dto.Id))
                .ReturnsAsync(new ResultModel<Order> { Data = order });

            _orderServiceMock.Setup(s => s.UpdateAsync(order))
                .ReturnsAsync(new ResultModel<Order> { Data = order });

            // Act
            var result = await _controller.Update(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Empty(apiResponse.Errors);
            Assert.Equal("Bestelling werd geüpdatet.", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Status", "Required");

            var dto = new OrderUpdateRequestDto();

            // Act
            var result = await _controller.Update(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Required", apiResponse.Errors);
            Assert.Equal("Ongeldige invoer.", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenOrderNotFound_ReturnsNotFound()
        {
            // Arrange
            var dto = new OrderUpdateRequestDto { Id = Guid.NewGuid(), Status = OrderStatus.Afgehaald };

            _orderServiceMock.Setup(s => s.GetByIdAsync(dto.Id))
                .ReturnsAsync(new ResultModel<Order> { Data = null, Errors = new List<string> { "Not found" } });

            // Act
            var result = await _controller.Update(dto);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(notFound.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Not found", apiResponse.Errors);
            Assert.Equal("Bestelling werd niet gevonden.", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenOrderServiceReturnsErrors_ReturnsBadRequest()
        {
            // Arrange
            var order = new Order("UserId", "Order1", 50, 2) { Status = OrderStatus.Besteld };

            var dto = new OrderUpdateRequestDto { Id = order.Id, Status = OrderStatus.Afgehaald };

            _orderServiceMock.Setup(s => s.GetByIdAsync(dto.Id))
                .ReturnsAsync(new ResultModel<Order> { Data = order });

            _orderServiceMock.Setup(s => s.UpdateAsync(order))
                .ReturnsAsync(new ResultModel<Order> { Errors = new List<string> { "Service error" } });

            // Act
            var result = await _controller.Update(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Service error", apiResponse.Errors);
            Assert.Equal("Updaten van bestelling is niet gelukt.", apiResponse.Message);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task Delete_WhenOrderDeleted_ReturnsOk()
        {
            // Arrange
            var user = new ApplicationUser("test", "person", "testperson@hotmail.com");

            var order = new Order(user.Id, "Order1", 90, 2);

            _orderServiceMock.Setup(s => s.GetByIdAsync(order.Id))
                .ReturnsAsync(new ResultModel<Order> { Data = order });

            _orderItemServiceMock.Setup(s => s.DeleteAsync(It.IsAny<OrderItem>()))
                .ReturnsAsync(new ResultModel<OrderItem>());

            _orderServiceMock.Setup(s => s.DeleteAsync(order))
                .ReturnsAsync(new ResultModel<Order>());

            // Act
            var result = await _controller.Delete(order.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Empty(apiResponse.Errors);
            Assert.Equal("Bestelling is verwijderd.", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenOrderNotFound_ReturnsBadRequest()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            _orderServiceMock.Setup(s => s.GetByIdAsync(orderId))
                .ReturnsAsync(new ResultModel<Order> { Data = null, Errors = new List<string> { "Not found" } });

            // Act
            var result = await _controller.Delete(orderId);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Not found", apiResponse.Errors);
            Assert.Equal("Bestelling niet gevonden.", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenOrderServiceReturnsErrors_ReturnsBadRequest()
        {
            // Arrange
            var user = new ApplicationUser("test", "person", "testperson@hotmail.com");

            var order = new Order(user.Id, "Order1", 90, 2);

            _orderServiceMock.Setup(s => s.GetByIdAsync(order.Id))
                .ReturnsAsync(new ResultModel<Order> { Data = order });

            _orderServiceMock.Setup(s => s.DeleteAsync(order))
                .ReturnsAsync(new ResultModel<Order> { Errors = new List<string> { "Service error" } });

            // Act
            var result = await _controller.Delete(order.Id);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal("Verwijderen van bestelling is niet gelukt.", apiResponse.Message);
        }
        #endregion
    }
}
