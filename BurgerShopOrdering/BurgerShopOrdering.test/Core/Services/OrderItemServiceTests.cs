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
    public class OrderItemServiceTests
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
        public async Task GetAllAsync_WhenOrderItemsExist_ReturnsOrderItems()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var user = new ApplicationUser("test", "person", "testperson@hotmail.com");
            var order = new Order(user.Id, "Bestelling test", 10.0m, 2, null);
            var product = new Product("Burger", 5.0m);
            var orderItem = new OrderItem(order.Id, product.Id, 2, 5.0m);

            context.Users.Add(user);
            context.Orders.Add(order);
            context.Products.Add(product);
            context.OrderItems.Add(orderItem);
            await context.SaveChangesAsync();

            var service = new OrderItemService(context);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.Equal(1, await context.OrderItems.CountAsync());
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Errors);
            Assert.Single(result.Data);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoOrderItems_ReturnsError()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new OrderItemService(context);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.Contains("Er zijn geen beschikbare order items", result.Errors);
        }

        #endregion

        #region GetByIdAsync

        [Fact]
        public async Task GetByIdAsync_WhenOrderItemExists_ReturnsOrderItem()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var user = new ApplicationUser("test", "person", "testperson@hotmail.com");
            var order = new Order(user.Id, "Bestelling test", 10.0m, 2, null);
            var product = new Product("Burger", 5.0m);
            var orderItem = new OrderItem(order.Id, product.Id, 2, 5.0m);

            context.Users.Add(user);
            context.Orders.Add(order);
            context.Products.Add(product);
            context.OrderItems.Add(orderItem);
            await context.SaveChangesAsync();

            var service = new OrderItemService(context);

            // Act
            var result = await service.GetByIdAsync(orderItem.Id);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Equal(orderItem.Id, result.Data.Id);
        }

        [Fact]
        public async Task GetByIdAsync_WhenOrderItemNotFound_ReturnsError()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new OrderItemService(context);

            // Act
            var result = await service.GetByIdAsync(Guid.NewGuid());

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.Contains("Er bestaat geen order item met dit id", result.Errors);
        }

        #endregion

        #region AddAsync

        [Fact]
        public async Task AddAsync_AddsOrderItem()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var order = new Order("user1", "Bestelling test", 10.0m, 2, null);
            var product = new Product("Burger", 5.0m);

            context.Orders.Add(order);
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var service = new OrderItemService(context);
            var orderItem = new OrderItem(order.Id, product.Id, 2, 5.0m);

            // Act
            var result = await service.AddAsync(orderItem);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);

            var itemInDb = await context.OrderItems.FindAsync(orderItem.Id);
            Assert.NotNull(itemInDb);
        }

        #endregion

        #region DeleteAsync

        [Fact]
        public async Task DeleteAsync_DeletesOrderItem()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var order = new Order("user1", "Bestelling test", 10.0m, 2, null);
            var product = new Product("Burger", 5.0m);
            var orderItem = new OrderItem(order.Id, product.Id, 2, 5.0m);

            context.Orders.Add(order);
            context.Products.Add(product);
            context.OrderItems.Add(orderItem);
            await context.SaveChangesAsync();

            var service = new OrderItemService(context);

            // Act
            var result = await service.DeleteAsync(orderItem);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);

            var deleted = await context.OrderItems.FindAsync(orderItem.Id);
            Assert.Null(deleted);
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_UpdatesOrderItem()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var order = new Order("user1", "Bestelling test", 10.0m, 2, null);
            var product = new Product("Burger", 5.0m);
            var orderItem = new OrderItem(order.Id, product.Id, 2, 5.0m);

            context.Orders.Add(order);
            context.Products.Add(product);
            context.OrderItems.Add(orderItem);
            await context.SaveChangesAsync();

            var service = new OrderItemService(context);

            orderItem.Quantity = 10;

            // Act
            var result = await service.UpdateAsync(orderItem);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Equal(10, result.Data.Quantity);

            var updated = await context.OrderItems.FindAsync(orderItem.Id);
            Assert.Equal(10, updated.Quantity);
        }

        #endregion
    }
}
