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
    public class CategoryServiceTests
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
        public async Task GetAllAsync_WhenCategoriesExist_ReturnsCategories()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Categories.Add(new Category("Vlees"));
            context.Categories.Add(new Category("Groenten"));
            await context.SaveChangesAsync();

            var service = new CategoryService(context);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Errors);
            Assert.Equal(2, result.Data.Count());
        }
        [Fact]
        public async Task GetAllAsync_WhenNoCategories_ReturnsError()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new CategoryService(context);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.Contains("Er zijn geen beschikbare categorieën", result.Errors);
        }

        #endregion

        #region GetAllVisibleCategories Tests

        [Fact]
        public async Task GetAllVisibleCategories_WhenVisibleCategoriesExist_ReturnsVisibleCategories()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Categories.Add(new Category("Vlees", true));
            context.Categories.Add(new Category("Groenten", false));
            await context.SaveChangesAsync();

            var service = new CategoryService(context);

            // Act
            var result = await service.GetAllVisibleCategories();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Errors);
            Assert.Single(result.Data);
            Assert.All(result.Data, c => Assert.True(c.IsVisible));
        }
        [Fact]
        public async Task GetAllVisibleCategories_WhenNoVisibleCategories_ReturnsError()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Categories.Add(new Category("Vlees", false));
            await context.SaveChangesAsync();

            var service = new CategoryService(context);

            // Act
            var result = await service.GetAllVisibleCategories();

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.Contains("Er zijn geen beschikbare categorieën", result.Errors);
        }

        #endregion

        #region GetByIdAsync Tests

        [Fact]
        public async Task GetByIdAsync_WhenCategoryFound_ReturnsCategory()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var category = new Category("Vlees");
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            var service = new CategoryService(context);

            // Act
            var result = await service.GetByIdAsync(category.Id);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Errors);
            Assert.Equal(category.Id, result.Data.Id);
        }
        [Fact]
        public async Task GetByIdAsync_WhenCategoryNotFound_ReturnsError()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new CategoryService(context);
            var randomId = Guid.NewGuid();

            // Act
            var result = await service.GetByIdAsync(randomId);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.Contains("Er is geen categorie met dit id", result.Errors);
        }

        #endregion

        #region AddAsync Tests

        [Fact]
        public async Task AddAsync_WhenCategoryNameExistsButNotVisible_MakesVisible()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var existing = new Category("Vlees", false);
            context.Categories.Add(existing);
            await context.SaveChangesAsync();

            var service = new CategoryService(context);
            var newCategory = new Category("Vlees");

            // Act
            var result = await service.AddAsync(newCategory);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.True(result.Data.IsVisible);
            Assert.Empty(result.Errors);
        }
        [Fact]
        public async Task AddAsync_WhenCategoryNameDoesNotExist_AddsCategory()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new CategoryService(context);
            var newCategory = new Category("Vis");

            // Act
            var result = await service.AddAsync(newCategory);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Equal("Vis", result.Data.Name);
            Assert.Empty(result.Errors);
        }
        [Fact]
        public async Task AddAsync_WhenCategoryNameExistsAndVisible_ReturnsError()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Categories.Add(new Category("Vlees", true));
            await context.SaveChangesAsync();

            var service = new CategoryService(context);
            var newCategory = new Category("Vlees");

            // Act
            var result = await service.AddAsync(newCategory);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.Contains($"Er bestaat al een categorie met de naam {newCategory.Name}", result.Errors);
        }

        #endregion

        #region DeleteAsync Tests

        [Fact]
        public async Task DeleteAsync_WhenNoProductsInCategory_RemovesCategory()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var category = new Category("Vlees");
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            var service = new CategoryService(context);

            // Act
            var result = await service.DeleteAsync(category);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Errors);

            var categoryInDb = await context.Categories.FindAsync(category.Id);
            Assert.Null(categoryInDb);
        }

        [Fact]
        public async Task DeleteAsync_WhenProductsInCategoryAndVisible_ReturnsError()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var category = new Category("Vlees", true);
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            var product = new Product("Burger", 3.50M, true);
            product.Categories.Add(category);
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var service = new CategoryService(context);

            // Act
            var result = await service.DeleteAsync(category);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.Contains("Er bevinden zich nog producten in deze categorie, verwijder deze eerst", result.Errors);

            var categoryInDb = await context.Categories.FindAsync(category.Id);
            Assert.NotNull(categoryInDb);
            Assert.True(categoryInDb.IsVisible);
        }

        [Fact]
        public async Task DeleteAsync_WhenProductsInCategoryButNotVisible_MakesInvisible()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var category = new Category("Vlees", true);
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            var product = new Product("Burger", 3.50M, false);
            product.Categories.Add(category);
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var service = new CategoryService(context);

            // Act
            var result = await service.DeleteAsync(category);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Errors);
            Assert.False(result.Data.IsVisible);

            var categoryInDb = await context.Categories.FindAsync(category.Id);
            Assert.NotNull(categoryInDb);
            Assert.False(categoryInDb.IsVisible);
        }

        #endregion

        #region UpdateAsync Tests

        [Fact]
        public async Task UpdateAsync_UpdatesCategory()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var category = new Category("Vlees");
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            var service = new CategoryService(context);

            category.Name = "Vega";

            // Act
            var result = await service.UpdateAsync(category);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Equal("Vega", result.Data.Name);

            var updatedCategory = await context.Categories.FindAsync(category.Id);
            Assert.Equal("Vega", updatedCategory.Name);
        }

        #endregion
    }
}
