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
    public class ProductServiceTests
    {
        private BurgerShopDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<BurgerShopDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new BurgerShopDbContext(options);
        }

        #region GetAllAsync Tests
        [Fact]
        public async Task GetAllAsync_WhenProductsExist_ReturnsProducts()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            context.Products.Add(new Product("Burger1", 3.40M));
            context.Products.Add(new Product("Burger2", 5.60m));
            await context.SaveChangesAsync();

            var service = new ProductService(context);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Errors);
            Assert.Equal(2, result.Data.Count());
        }
        [Fact]
        public async Task GetAllAsync_WhenNoProducts_ReturnsError()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new ProductService(context);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.Contains("Er zijn geen beschikbare producten", result.Errors);
        }
        #endregion
        #region GetAllVisibleProducts Tests
        [Fact]
        public async Task GetAllVisibleProducts_WhenVisibleProductsExist_ReturnsVisibleProducts()
        {
            //Arrange
            var context = GetInMemoryDbContext();
            context.Products.Add(new Product("Burger1", 3.40M, true));
            context.Products.Add(new Product("Burger2", 5.60M, false));
            await context.SaveChangesAsync();

            var service = new ProductService(context);

            //Act
            var result = await service.GetAllVisibleProducts();

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Errors);
            Assert.Single(result.Data);
            Assert.All(result.Data, p => Assert.True(p.IsVisible));
        }
        [Fact]
        public async Task GetAllVisibleProducts_WhenNoVisibleProducts_ReturnsError()
        {
            //Arrange
            var context = GetInMemoryDbContext();
            context.Products.Add(new Product("Burger1", 3.40M, false));
            await context.SaveChangesAsync();

            var service = new ProductService(context);

            //Act
            var result = await service.GetAllVisibleProducts();

            //Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.Contains("Er zijn geen beschikbare producten", result.Errors);
        }
        #endregion
        #region GetByIdAsync Tests
        [Fact]
        public async Task GetByIdAsync_WhenProductFound_ReturnsProduct()
        {
            //Arrange
            var context = GetInMemoryDbContext();
            var product = new Product("Burger1", 3.40M);
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var service = new ProductService(context);

            //Act
            var result = await service.GetByIdAsync(product.Id);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Errors);
            Assert.Equal(product.Id, result.Data.Id);
        }
        [Fact]
        public async Task GetByIdAsync_WhenProductNotFound_ReturnsError()
        {
            //Arrange
            var context = GetInMemoryDbContext();
            var service = new ProductService(context);
            var randomId = Guid.NewGuid();

            //Act
            var result = await service.GetByIdAsync(randomId);

            //Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.Contains("Er is geen product met dit id", result.Errors);
        }
        #endregion
        #region AddAsync Tests
        [Fact]
        public async Task AddAsync_WhenProductNameExistsButNotVisible_MakesVisibleAndUpdatesPriceAndImage()
        {
            //Arrange
            var context = GetInMemoryDbContext();
            var existing = new Product("Burger1", 3.40M, false) { Image = "old.jpg" };
            context.Products.Add(existing);
            await context.SaveChangesAsync();

            var service = new ProductService(context);
            var newProduct = new Product("Burger1", 4.50M) { Image = "new.jpg" };

            //Act
            var result = await service.AddAsync(newProduct);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.True(result.Data.IsVisible);
            Assert.Equal(4.50M, result.Data.Price);
            Assert.Equal("new.jpg", result.Data.Image);
            Assert.Empty(result.Errors);
        }
        [Fact]
        public async Task AddAsync_WhenProductNameDoesNotExist_AddsProduct()
        {
            //Arrange
            var context = GetInMemoryDbContext();
            var service = new ProductService(context);
            var newProduct = new Product("NewBurger", 2.50M);

            //Act
            var result = await service.AddAsync(newProduct);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Equal("NewBurger", result.Data.Name);
            Assert.Empty(result.Errors);
        }
        [Fact]
        public async Task AddAsync_WhenProductNameExistsAndVisible_ReturnsError()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Products.Add(new Product("Burger1", 3.40M, true));
            await context.SaveChangesAsync();

            var service = new ProductService(context);

            var newProduct = new Product("Burger1", 3.60M);

            // Act
            var result = await service.AddAsync(newProduct);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.Contains($"Er bestaat al een product met de naam {newProduct.Name}", result.Errors);
        }
        #endregion
        #region DeleteAsync Tests
        [Fact]
        public async Task DeleteAsync_WhenProductNotInOrderItems_RemovesProduct()
        {
            //Arrange
            var context = GetInMemoryDbContext();

            var product = new Product("Burger1", 3.40M);
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var service = new ProductService(context);

            //Act
            var result = await service.DeleteAsync(product);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Errors);

            var productInDb = await context.Products.FindAsync(product.Id);
            Assert.Null(productInDb);
        }
        [Fact]
        public async Task DeleteAsync_WhenProductInOrderItems_MakesInvisible()
        {
            //Arrange
            var context = GetInMemoryDbContext();
            var product = new Product("Burger1", 3.40M);
            context.Products.Add(product);
            await context.SaveChangesAsync();

            context.OrderItems.Add(new OrderItem(Guid.NewGuid(), product.Id, 1, 3.40M));
            await context.SaveChangesAsync();

            var service = new ProductService(context);

            //Act
            var result = await service.DeleteAsync(product);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.False(result.Data.IsVisible);
            Assert.Empty(result.Errors);

            var productInDb = await context.Products.FindAsync(product.Id);
            Assert.NotNull(productInDb);
            Assert.False(productInDb.IsVisible);
        }
        #endregion
        #region UpdateAsync Tests
        [Fact]
        public async Task UpdateAsync_UpdatesProduct()
        {
            //Arrange
            var context = GetInMemoryDbContext();

            var product = new Product("Burger1", 3.40M);
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var service = new ProductService(context);

            product.Price = 5.00M;

            //Act
            var result = await service.UpdateAsync(product);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Equal(5.00M, result.Data.Price);

            var updatedProduct = await context.Products.FindAsync(product.Id);
            Assert.Equal(5.00M, updatedProduct.Price);
        }
        #endregion

    }
}
