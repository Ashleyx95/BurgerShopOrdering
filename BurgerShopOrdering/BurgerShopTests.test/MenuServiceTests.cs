using BurgerShopApiConsumer.Categories;
using BurgerShopApiConsumer.Categories.Models;
using BurgerShopApiConsumer.Common;
using BurgerShopApiConsumer.Products;
using BurgerShopApiConsumer.Products.Model;
using BurgerShopOrdering.Core.Models;
using BurgerShopOrdering.Core.Services.Interfaces;
using BurgerShopOrdering.Core.Services.Web;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopTests.test
{
    public class MenuServiceTests
    {
        private readonly Mock<IProductApiService> _productApiServiceMock = new();
        private readonly Mock<ICategoryApiService> _categoryApiServiceMock = new();
        private readonly Mock<IAccountService> _accountServiceMock = new();
        private readonly IMenuService _menuService;

        public MenuServiceTests()
        {
            _menuService = new MenuService(
                _productApiServiceMock.Object,
                _categoryApiServiceMock.Object,
                _accountServiceMock.Object);
        }

        [Fact]
        public async Task GetCategoriesAsync_WhenApiReturnsSuccess_ReturnsCategories()
        {
            //Arrange
            var categories = new[] { new CategoryResponseApiModel { Id = Guid.NewGuid(), Name = "Burgers" } };

            _categoryApiServiceMock.Setup(x => x.GetCategoriesAsync())
                .ReturnsAsync(ApiResponse<CategoryResponseApiModel[]>.SuccessResponse(categories));

            //Act
            var result = await _menuService.GetCategoriesAsync();

            //Assert
            Assert.Single(result);
            Assert.Equal("Burgers", result.First().Name);
        }
        [Fact]
        public async Task GetCategoriesAsync_WhenApiFails_ReturnsEmpty()
        {
            //Arrange
            _categoryApiServiceMock.Setup(x => x.GetCategoriesAsync())
                .ReturnsAsync(ApiResponse<CategoryResponseApiModel[]>.FailureResponse("error"));

            //Act
            var result = await _menuService.GetCategoriesAsync();

            //Assert
            Assert.Empty(result);
        }
        //[Fact]
        //public async Task GetProductsAsync_WhenApiReturnsSucces_ReturnsProducts()
        //{
        //    //Arrange
        //    var categoryName = "Burgers";
        //    var catId = Guid.NewGuid();

        //    _categoryApiServiceMock.Setup(x => x.GetCategoriesAsync())
        //        .ReturnsAsync(ApiResponse <CategoryResponseApiModel[]>.SuccessResponse([
        //            new CategoryResponseApiModel { Id = catId, Name = categoryName }
        //        ]));

        //    _productApiServiceMock.Setup(x => x.GetProductsAsync())
        //        .ReturnsAsync(ApiResponse<ProductResponseApiModel[]>.SuccessResponse([
        //            new ProductResponseApiModel {
        //            Id = Guid.NewGuid(),
        //            Name = "Burger",
        //            Price = 8,
        //            Image = "img.jpg",
        //            Categories = [categoryName]
        //        }
        //        ]));

        //    //Act
        //    var result = await _menuService.GetProductsAsync();

        //    //Assert
        //    Assert.Single(result);
        //    Assert.Equal("Burger", result.First().Name);
        //    Assert.Equal(8, result.First().Price);
        //    Assert.Single(result.First().Categories);
        //    Assert.Equal(categoryName, result.First().Categories.First().Name);
        //}
        [Fact]
        public async Task GetProductsAsync_WhenApiFails_ReturnsEmpty()
        {
            //Arrange
            _productApiServiceMock.Setup(x => x.GetProductsAsync())
                .ReturnsAsync(ApiResponse<ProductResponseApiModel[]>.FailureResponse("error"));

            //Act
            var result = await _menuService.GetProductsAsync();

            //Assert
            Assert.Empty(result);
        }
        [Fact]
        public async Task AddCategory_WhenApiReturnsSucces_ReturnsSuccessResult()
        {
            //Arrange
            _accountServiceMock.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");
            _categoryApiServiceMock.Setup(x => x.CreateCategoryAsync(It.IsAny<CategoryCreateRequestApiModel>(), "token"))
                .ReturnsAsync(ApiResponse<object>.SuccessResponse(null));

            //Act
            var result = await _menuService.AddCategory("Test");

            //Assert
            Assert.True(result.Success);
        }
        [Fact]
        public async Task AddCategory_WhenApiFails_ReturnsFailResult()
        {
            //Arrange
            _accountServiceMock.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");
            _categoryApiServiceMock.Setup(x => x.CreateCategoryAsync(It.IsAny<CategoryCreateRequestApiModel>(), "token"))
                .ReturnsAsync(ApiResponse<object>.FailureResponse("Error"));

            //Act
            var result = await _menuService.AddCategory("Test");

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Error", result.Message);
        }
        [Fact]
        public async Task AddNewProductToMenu_WhenApiReturnsSucces_ReturnsSuccessResult()
        {
            //Arrange
            var product = new Product
            {
                Name = "Test",
                Price = 10,
                Image = "",
                Categories = [new Category { Id = Guid.NewGuid(), Name = "Test" }]
            };

            _accountServiceMock.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");
            _productApiServiceMock.Setup(x => x.CreateProductAsync(It.IsAny<ProductCreateRequestApiModel>(), "token"))
                .ReturnsAsync(ApiResponse<object>.SuccessResponse(null));

            //Act
            var result = await _menuService.AddNewProductToMenu(product);

            //Assert
            Assert.True(result.Success);
        }
        [Fact]
        public async Task AddNewProductToMenu_WhenApiFails_ReturnsFailResult()
        {
            //Arrange
            var product = new Product
            {
                Name = "Test",
                Price = 10,
                Image = "",
                Categories = [new Category { Id = Guid.NewGuid(), Name = "Test" }]
            };

            _accountServiceMock.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");
            _productApiServiceMock.Setup(x => x.CreateProductAsync(It.IsAny<ProductCreateRequestApiModel>(), "token"))
                .ReturnsAsync(ApiResponse<object>.FailureResponse("Error"));

            //Act
            var result = await _menuService.AddNewProductToMenu(product);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Error", result.Message);
        }
        [Fact]
        public async Task RemoveCategory_WhenApiReturnsSucces_ReturnsSuccesResult()
        {
            //Arrange
            var cat = new Category { Id = Guid.NewGuid(), Name = "Test" };

            _accountServiceMock.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");
            _categoryApiServiceMock.Setup(x => x.DeleteCategoryAsync(cat.Id, "token"))
                .ReturnsAsync(ApiResponse<object>.SuccessResponse(null));

            //Act
            var result = await _menuService.RemoveCategory(cat);

            //Assert
            Assert.True(result.Success);
        }
        [Fact]
        public async Task RemoveCategory_WhenApiFails_ReturnsFailResult()
        {
            //Arrange
            var cat = new Category { Id = Guid.NewGuid(), Name = "Test" };

            _accountServiceMock.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");
            _categoryApiServiceMock.Setup(x => x.DeleteCategoryAsync(cat.Id, "token"))
                .ReturnsAsync(ApiResponse<object>.FailureResponse("Error"));

            //Act
            var result = await _menuService.RemoveCategory(cat);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Error", result.Message);
        }
        [Fact]
        public async Task RemoveProductFromMenu_WhenApiReturnsSucces_ReturnsSuccesResult()
        {
            //Arrange
            var prod = new Product { Id = Guid.NewGuid() };

            _accountServiceMock.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");
            _productApiServiceMock.Setup(x => x.DeleteProductAsync(prod.Id, "token"))
                .ReturnsAsync(ApiResponse<object>.SuccessResponse(null));

            //Act
            var result = await _menuService.RemoveProductFromMenu(prod);

            //Assert
            Assert.True(result.Success);
        }
        [Fact]
        public async Task RemoveProductFromMenu_WhenApiFails_ReturnsFailResult()
        {
            //Arrange
            var prod = new Product { Id = Guid.NewGuid() };

            _accountServiceMock.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");
            _productApiServiceMock.Setup(x => x.DeleteProductAsync(prod.Id, "token"))
                .ReturnsAsync(ApiResponse<object>.FailureResponse("Error"));

            //Act
            var result = await _menuService.RemoveProductFromMenu(prod);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Error", result.Message);
        }
        [Fact]
        public async Task UpdateProductToMenu_WhenApiReturnsSucces_ReturnsSuccessResult()
        {
            //Arrange
            var prod = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Updated",
                Price = 15,
                Image = null,
                Categories = [new Category { Id = Guid.NewGuid(), Name = "Test" }]
            };

            _accountServiceMock.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");
            _productApiServiceMock.Setup(x => x.UpdateProductAsync(It.IsAny<ProductUpdateRequestApiModel>(), "token"))
                .ReturnsAsync(ApiResponse<object>.SuccessResponse(null));

            //Act
            var result = await _menuService.UpdateProductToMenu(prod);

            //Assert
            Assert.True(result.Success);
        }
        [Fact]
        public async Task UpdateProductToMenu_WhenApiFails_ReturnsFailResult()
        {
            //Arrange
            var prod = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Updated",
                Price = 15,
                Image = null,
                Categories = [new Category { Id = Guid.NewGuid(), Name = "Test" }]
            };

            _accountServiceMock.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");
            _productApiServiceMock.Setup(x => x.UpdateProductAsync(It.IsAny<ProductUpdateRequestApiModel>(), "token"))
                .ReturnsAsync(ApiResponse<object>.FailureResponse("Error"));

            //Act
            var result = await _menuService.UpdateProductToMenu(prod);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Error", result.Message);
        }
        [Theory]
        [InlineData("Test File.jpg", "test_file.jpg")]
        [InlineData("FileName?Test.png", "filename_test.png")]
        [InlineData("ValidName123.png", "validname123.png")]
        public void MakeFileNameSafe_ReplacesInvalidCharacters_AndConvertsToLower(string input, string expected)
        {
            // Act
            var result = _menuService.MakeFileNameSafe(input);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
