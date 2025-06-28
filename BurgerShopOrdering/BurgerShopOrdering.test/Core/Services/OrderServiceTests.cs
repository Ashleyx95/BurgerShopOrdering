using BurgerShopOrdering.core.Data;
using BurgerShopOrdering.core.Entities;
using BurgerShopOrdering.core.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.test.Core.Services
{
    public class OrderServiceTests
    {
        private BurgerShopDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<BurgerShopDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new BurgerShopDbContext(options);
        }

        #region GetAllAsync
        [Fact]
        public async Task GetAllAsync_WhenOrdersExist_ReturnsOrders()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var user = new ApplicationUser("test", "person", "testperson@hotmail.com");
            var order = new Order(user.Id, "Order1", 10.0m, 2);
            context.Users.Add(user);
            context.Orders.Add(order);
            await context.SaveChangesAsync();

            var service = new OrderService(context);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Errors);
            Assert.Single(result.Data);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoOrders_ReturnsError()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new OrderService(context);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.Contains("Er zijn geen beschikbare bestellingen", result.Errors);
        }
        #endregion

        #region GetByIdAsync
        [Fact]
        public async Task GetByIdAsync_WhenOrderExists_ReturnsOrder()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var user = new ApplicationUser("test", "person", "testperson@hotmail.com");
            var order = new Order(user.Id, "Order1", 10.0m, 2);
            context.Users.Add(user);
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            var service = new OrderService(context);

            // Act
            var result = await service.GetByIdAsync(order.Id);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Equal(order.Id, result.Data.Id);
        }

        [Fact]
        public async Task GetByIdAsync_WhenOrderDoesNotExist_ReturnsError()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new OrderService(context);

            // Act
            var result = await service.GetByIdAsync(Guid.NewGuid());

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.Contains("Er bestaat geen bestelling met dit id", result.Errors);
        }
        #endregion

        #region GetByStatusAsync
        [Fact]
        public async Task GetByStatusAsync_WhenOrdersExistWithStatus_ReturnsOrders()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var user = new ApplicationUser("test", "person", "testperson@hotmail.com");
            var order = new Order(user.Id, "Order1", 10.0m, 2) { Status = OrderStatus.Besteld };
            context.Users.Add(user);
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            var service = new OrderService(context);

            // Act
            var result = await service.GetByStatusAsync(OrderStatus.Besteld);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Single(result.Data);
        }

        [Fact]
        public async Task GetByStatusAsync_WhenNoOrdersWithStatus_ReturnsError()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new OrderService(context);

            // Act
            var result = await service.GetByStatusAsync(OrderStatus.Besteld);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.Contains("Er zijn geen bestellingen met status", result.Errors.First());
        }
        #endregion

        #region GetOrdersByUserAsync
        [Fact]
        public async Task GetOrdersByUserAsync_WhenOrdersExistForUser_ReturnsOrders()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var user = new ApplicationUser("test", "person", "testperson@hotmail.com");
            var order = new Order(user.Id, "Order1", 10.0m, 2);
            context.Users.Add(user);
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            var service = new OrderService(context);

            // Act
            var result = await service.GetOrdersByUserAsync(user.Id);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Single(result.Data);
        }

        [Fact]
        public async Task GetOrdersByUserAsync_WhenNoOrdersForUser_ReturnsError()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new OrderService(context);

            // Act
            var result = await service.GetOrdersByUserAsync("nonexistent-user");

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.Contains("Er zijn geen bestellingen van deze gebruiker", result.Errors);
        }
        #endregion

        #region GetOrdersByUserAndStatusAsync
        [Fact]
        public async Task GetOrdersByUserAndStatusAsync_WhenOrdersExistForUserAndStatus_ReturnsOrders()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var user = new ApplicationUser("test", "person", "testperson@hotmail.com");
            var order = new Order(user.Id, "Order1", 10.0m, 2) { Status = OrderStatus.Afgehaald};
            context.Users.Add(user);
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            var service = new OrderService(context);

            // Act
            var result = await service.GetOrdersByUserAndStatusAsync(user.Id, OrderStatus.Afgehaald);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Single(result.Data);
        }

        [Fact]
        public async Task GetOrdersByUserAndStatusAsync_WhenNoOrdersForUserAndStatus_ReturnsError()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new OrderService(context);

            // Act
            var result = await service.GetOrdersByUserAndStatusAsync("nonexistent-user", OrderStatus.Afgehaald);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.Contains($"Er zijn geen bestellingen van deze gebruiker met status {OrderStatus.Afgehaald}", result.Errors);
        }
        #endregion

        #region AddAsync
        [Fact]
        public async Task AddAsync_AddsOrderSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var order = new Order("user1", "Order1", 10.0m, 2);
            var service = new OrderService(context);

            // Act
            var result = await service.AddAsync(order);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Equal(order.Id, result.Data.Id);
        }
        #endregion

        #region UpdateAsync
        [Fact]
        public async Task UpdateAsync_UpdatesOrderSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var order = new Order("user1", "Order1", 10.0m, 2);
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            var service = new OrderService(context);

            order.Status = OrderStatus.Afgehaald;

            // Act
            var result = await service.UpdateAsync(order);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(OrderStatus.Afgehaald, result.Data.Status);
        }
        #endregion

        #region DeleteAsync
        [Fact]
        public async Task DeleteAsync_WhenNoOrderItems_DeletesOrder()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var order = new Order("user1", "Order1", 10.0m, 2);
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            var service = new OrderService(context);

            // Act
            var result = await service.DeleteAsync(order);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Equal(order.Id, result.Data.Id);
        }

        [Fact]
        public async Task DeleteAsync_WhenOrderItemsExist_ReturnsError()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var order = new Order("user1", "Order1", 10.0m, 2);
            var product = new Product("Burger", 5.0m);
            var orderItem = new OrderItem(order.Id, product.Id, 2, 5.0m);

            context.Orders.Add(order);
            context.Products.Add(product);
            context.OrderItems.Add(orderItem);
            await context.SaveChangesAsync();

            var service = new OrderService(context);

            // Act
            var result = await service.DeleteAsync(order);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.Contains("Gelieve eerst de order items van deze bestelling te verwijderen", result.Errors);
        }
        #endregion
    }
}
