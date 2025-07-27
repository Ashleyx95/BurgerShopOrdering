using BurgerShopApiConsumer.Common;
using BurgerShopApiConsumer.Orders;
using BurgerShopApiConsumer.Orders.Model;
using BurgerShopOrdering.Core.Models;
using BurgerShopOrdering.Core.Services.Interfaces;
using BurgerShopOrdering.Core.Services.Web;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderStatus = BurgerShopOrdering.Core.Models.OrderStatus;

namespace BurgerShopTests.test
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderApiService> _orderApiServiceMock = new();
        private readonly Mock<IAccountService> _accountServiceMock = new();
        private readonly IOrderService _orderService;

        public OrderServiceTests()
        {
            _orderService = new OrderService(_orderApiServiceMock.Object, _accountServiceMock.Object);
        }

        [Fact]
        public async Task GetOrdersAsync_WhenApiReturnsSuccess_ReturnsOrders()
        {
            // Arrange
            var orderResponse = new OrderResponseApiModel
            {
                Id = Guid.NewGuid(),
                Name = "Test Order",
                TotalPrice = 10,
                TotalQuantity = 2,
                DateOrdered = DateTime.UtcNow,
                NameUser = "John",
                Status = "Besteld",
                OrderItems = new List<OrderItemResponseApiModel>
                {
                    new() { ProductName = "Burger", Price = 5, Quantity = 2 }
                }
            };

            _orderApiServiceMock.Setup(x => x.GetOrdersAsync(It.IsAny<string>()))
                .ReturnsAsync(ApiResponse<OrderResponseApiModel[]>.SuccessResponse([orderResponse]));

            _accountServiceMock.Setup(x => x.GetTokenAsync())
                .ReturnsAsync("valid-token");

            // Act
            var result = await _orderService.GetOrdersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Test Order", result.First().Name);
            Assert.Equal(10, result.First().TotalPrice);
            Assert.Equal(2, result.First().TotalQuantity);
            Assert.NotNull(result.First().OrderItems);
            Assert.Single(result.First().OrderItems);
            Assert.Equal("Burger", result.First().OrderItems.First().ProductName);
        }
        [Fact]
        public async Task GetOrdersAsync_WhenApiFails_ReturnsEmptyList()
        {
            //Arrange
            _orderApiServiceMock.Setup(x => x.GetOrdersAsync(It.IsAny<string>()))
                .ReturnsAsync(ApiResponse<OrderResponseApiModel[]>.FailureResponse("Error"));

            _accountServiceMock.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            //Act
            var orders = await _orderService.GetOrdersAsync();

            //Assert
            Assert.NotNull(orders);
            Assert.Empty(orders);
        }
        [Fact]
        public async Task GetOrdersByStatusAsync_WhenApiReturnsSuccess_ReturnsOrders()
        {
            // Arrange
            var status = "Besteld";

            var orderResponse = new OrderResponseApiModel
            {
                Id = Guid.NewGuid(),
                Name = "Test Order",
                TotalPrice = 10,
                TotalQuantity = 2,
                DateOrdered = DateTime.UtcNow,
                NameUser = "John",
                Status = "Besteld",
                OrderItems = new List<OrderItemResponseApiModel>
                {
                    new() { ProductName = "Burger", Price = 5, Quantity = 2 }
                }
            };

            _orderApiServiceMock.Setup(x => x.GetOrdersByStatusAsync(status, It.IsAny<string>()))
                .ReturnsAsync(ApiResponse<OrderResponseApiModel[]>.SuccessResponse([orderResponse]));

            _accountServiceMock.Setup(x => x.GetTokenAsync())
                .ReturnsAsync("valid-token");

            // Act
            var result = await _orderService.GetOrdersByStatusAsync(status);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Test Order", result.First().Name);
            Assert.Equal(10, result.First().TotalPrice);
            Assert.Equal(2, result.First().TotalQuantity);
            Assert.NotNull(result.First().OrderItems);
            Assert.Single(result.First().OrderItems);
            Assert.Equal("Burger", result.First().OrderItems.First().ProductName);
        }
        [Fact]
        public async Task GetOrdersByStatusAsync_WhenApiFails_ReturnsEmptyList()
        {
            // Arrange
            var status = "Besteld";

            _orderApiServiceMock.Setup(x => x.GetOrdersByStatusAsync(status, It.IsAny<string>()))
                .ReturnsAsync(ApiResponse<OrderResponseApiModel[]>.FailureResponse("Error"));

            _accountServiceMock.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var orders = await _orderService.GetOrdersByStatusAsync(status);

            // Assert
            Assert.NotNull(orders);
            Assert.Empty(orders);
        }
        [Fact]
        public async Task CreateOrderAsync_WhenApiReturnsSucces_ReturnsSuccessResult()
        {
            //Arrange
            var order = new Order
            {
                TotalPrice = 20,
                TotalQuantity = 2,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = Guid.NewGuid(), ProductPrice = 10, Quantity = 2 }
                }
            };

            _accountServiceMock.Setup(x => x.GetLoggedInUserAsync()).ReturnsAsync(new User());
            _accountServiceMock.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");
            _orderApiServiceMock.Setup(x => x.CreateOrderAsync(It.IsAny<OrderCreateRequestApiModel>(), It.IsAny<string>()))
                .ReturnsAsync(ApiResponse<object>.SuccessResponse(null, "Success"));

            //Act
            var result = await _orderService.CreateOrderAsync(order);

            //Assert
            Assert.True(result.Success);
            Assert.Equal("Success", result.Message);
        }
        [Fact]
        public async Task CreateOrderAsync_WhenApiFailed_ReturnsFailureResult()
        {
            //Arrange
            var order = new Order { OrderItems = new List<OrderItem>() };

            _accountServiceMock.Setup(x => x.GetLoggedInUserAsync()).ReturnsAsync(new User());
            _accountServiceMock.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");
            _orderApiServiceMock.Setup(x => x.CreateOrderAsync(It.IsAny<OrderCreateRequestApiModel>(), It.IsAny<string>()))
                .ReturnsAsync(ApiResponse<object>.FailureResponse("Fail"));

            //Act
            var result = await _orderService.CreateOrderAsync(order);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Fail", result.Message);
        }
        [Fact]
        public async Task UpdateOrderStatusAsync_WhenApiReturnsSucces_ReturnsSuccessResult()
        {
            //Arrange
            var order = new Order { Id = Guid.NewGuid(), Status = OrderStatus.Besteld };

            _accountServiceMock.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");
            _orderApiServiceMock.Setup(x => x.UpdateOrderAsync(It.IsAny<OrderUpdateRequestApiModel>(), It.IsAny<string>()))
                .ReturnsAsync(ApiResponse<object>.SuccessResponse(null, "Updated"));

            //Act
            var result = await _orderService.UpdateOrderStatusAsync(order);

            //Assert
            Assert.True(result.Success);
            Assert.Equal("Updated", result.Message);
        }
        [Fact]
        public async Task UpdateOrderStatusAsync_WhenApiFailed_ReturnsFailure()
        {
            //Arrange
            var order = new Order { Id = Guid.NewGuid(), Status = OrderStatus.Besteld };

            _accountServiceMock.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");
            _orderApiServiceMock.Setup(x => x.UpdateOrderAsync(It.IsAny<OrderUpdateRequestApiModel>(), It.IsAny<string>()))
                .ReturnsAsync(ApiResponse<object>.FailureResponse("Fail"));

            //Act
            var result = await _orderService.UpdateOrderStatusAsync(order);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Fail", result.Message);
        }
        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(2, 3, 5)]
        [InlineData(1, 1, 2)]
        [InlineData(5, 5, 10)]
        public void CalculateTotalItemsInOrder_ReturnsCorrectCount(int qty1, int qty2, int expected)
        {
            // Arrange
            var order = new Order
            {
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Quantity = qty1 },
                    new OrderItem { Quantity = qty2 }
                }
            };

            // Act
            var count = _orderService.CalculateTotalItemsInOrder(order);

            // Assert
            Assert.Equal(expected, count);
        }
        [Theory]
        [InlineData(2, 5, 1, 10, 20)]
        [InlineData(1, 2.5, 2, 3.5, 9.5)]
        [InlineData(0, 0, 0, 0, 0)]
        [InlineData(3, 1, 2, 2, 7)]
        public void CalculateTotalPriceOrder_ReturnsCorrectTotal(int qty1, decimal price1, int qty2, decimal price2, decimal expected)
        {
            // Arrange
            var order = new Order
            {
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Quantity = qty1, ProductPrice = price1 },
                    new OrderItem { Quantity = qty2, ProductPrice = price2 }
                }
            };

            // Act
            var total = _orderService.CalculateTotalPriceOrder(order);

            // Assert
            Assert.Equal(expected, total);
        }
        [Theory]
        [InlineData(2.5, 4, 10)]
        [InlineData(0, 10, 0)]
        [InlineData(1.99, 3, 5.97)]
        [InlineData(10, 1, 10)]
        public void CalculateTotalPriceOrderItem_ReturnsCorrectValue(decimal price, int quantity, decimal expected)
        {
            // Act
            var result = _orderService.CalculateTotalPriceOrderItem(price, quantity);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
