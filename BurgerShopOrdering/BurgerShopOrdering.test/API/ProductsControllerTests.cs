using BurgerShopOrdering.api.Controllers;
using BurgerShopOrdering.api.Dtos.Common;
using BurgerShopOrdering.api.Dtos.Products;
using BurgerShopOrdering.core.Entities;
using BurgerShopOrdering.core.Services.Interfaces;
using BurgerShopOrdering.core.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.test.API
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService<Product>> _productServiceMock;
        private readonly Mock<ICategoryService<Category>> _categoryServiceMock;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _productServiceMock = new Mock<IProductService<Product>>();
            _categoryServiceMock = new Mock<ICategoryService<Category>>();
            _controller = new ProductsController(_productServiceMock.Object, _categoryServiceMock.Object);
        }

        #region Get
        [Fact]
        public async Task Get_WhenProductsExist_ReturnsOk_WithProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product("Burger", 5) { Categories = new List<Category> { new Category("Burgers") } }
            };
            _productServiceMock.Setup(s => s.GetAllVisibleProducts())
                .ReturnsAsync(new ResultModel<IEnumerable<Product>> { Data = products });

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<List<ProductResponseDto>>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Single(apiResponse.Data);
            Assert.Empty(apiResponse.Errors);
            Assert.Equal("Burger", apiResponse.Data[0].Name);
            Assert.Equal("Producten opgehaald", apiResponse.Message);
        }

        [Fact]
        public async Task Get_WhenProductServiceReturnsErrors_ReturnsBadRequest()
        {
            // Arrange
            _productServiceMock.Setup(s => s.GetAllVisibleProducts())
                .ReturnsAsync(new ResultModel<IEnumerable<Product>> { Errors = new List<string> { "Service error" } });

            // Act
            var result = await _controller.Get();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(badRequestResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Service error", apiResponse.Errors);
            Assert.Equal("Producten konden niet worden opgehaald.", apiResponse.Message);
        }

        [Fact]
        public async Task Get_ById_WhenProductFound_ReturnsOk()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product("Burger", 5) { Id = productId, Categories = new List<Category> { new Category("Burgers") } };
            _productServiceMock.Setup(s => s.GetByIdAsync(productId))
                .ReturnsAsync(new ResultModel<Product> { Data = product });

            // Act
            var result = await _controller.Get(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<ProductResponseDto>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal("Burger", apiResponse.Data.Name);
            Assert.Equal("Product opgehaald", apiResponse.Message);
        }

        [Fact]
        public async Task Get_ById_WhenProductNotFound_ReturnsNotFound()
        {
            // Arrange
            _productServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new ResultModel<Product> { Data = null, Errors = new List<string> { "Not found" } });

            // Act
            var result = await _controller.Get(Guid.NewGuid());

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Not found", apiResponse.Errors);
            Assert.Equal("Product werd niet gevonden.", apiResponse.Message);
        }
        #endregion

        #region Add

        [Fact]
        public async Task Add_WhenProductAdded_ReturnsCreatedAtAction()
        {
            // Arrange
            var category = new Category("Burgers");

            var dto = new ProductCreateRequestDto
            {
                Name = "Burger",
                Price = 10,
                CategoryIds = new List<Guid> { category.Id }
            };

            _categoryServiceMock.Setup(s => s.GetByIdAsync(category.Id))
                .ReturnsAsync(new ResultModel<Category> { Data = category });
            _productServiceMock.Setup(s => s.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync(new ResultModel<Product> { Data = new Product("Burger", 10) });

            // Act
            var result = await _controller.Add(dto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<ProductResponseDto>>(createdResult.Value);
            Assert.NotNull(apiResponse.Data);
            Assert.True(apiResponse.Success);
            Assert.Empty(apiResponse.Errors);
            Assert.Equal($"Product '{dto.Name}' is toegevoegd.", apiResponse.Message);
        }

        [Fact]
        public async Task Add_WhenModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");
            var dto = new ProductCreateRequestDto();

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
        public async Task Add_WhenNoCategories_ReturnsBadRequest()
        {
            // Arrange
            var dto = new ProductCreateRequestDto { Name = "Burger", Price = 10, CategoryIds = new List<Guid>() };

            // Act
            var result = await _controller.Add(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal("Minstens één categorie is vereist.", apiResponse.Message);
        }

        [Fact]
        public async Task Add_WhenCategoryNotFound_ReturnsBadRequest()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            var dto = new ProductCreateRequestDto
            {
                Name = "Burger",
                Price = 10,
                CategoryIds = new List<Guid> { categoryId }
            };
            _categoryServiceMock.Setup(s => s.GetByIdAsync(categoryId))
                .ReturnsAsync(new ResultModel<Category> { Data = null, Errors = new List<string> { "Not found" } });

            // Act
            var result = await _controller.Add(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Not found", apiResponse.Errors);
            Assert.Equal("Categorie werd niet gevonden.", apiResponse.Message);
        }

        [Fact]
        public async Task Add_WhenProductServiceReturnsErrors_ReturnsBadRequest()
        {
            // Arrange
            var category = new Category("Burgers");

            var dto = new ProductCreateRequestDto
            {
                Name = "Burger",
                Price = 10,
                Image = "burger.jpg",
                CategoryIds = new List<Guid> { category.Id }
            };

            _categoryServiceMock.Setup(s => s.GetByIdAsync(category.Id))
                .ReturnsAsync(new ResultModel<Category> { Data = category });

            _productServiceMock.Setup(s => s.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync(new ResultModel<Product> { Errors = new List<string> { "Service error" } });

            // Act
            var result = await _controller.Add(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Service error", apiResponse.Errors);
            Assert.Equal("Product kon niet worden toegevoegd.", apiResponse.Message);
        }

        #endregion

        #region Update
        [Fact]
        public async Task Update_WhenProductUpdated_ReturnsOk()
        {
            // Arrange
            var existingProduct = new Product("Burger", 10);
            var category = new Category("Burger");
        
            var dto = new ProductUpdateRequestDto
            {
                Id = existingProduct.Id,
                Name = "New Name",
                Price = 20,
                Image = "img.jpg",
                CategoryIds = new List<Guid> { category.Id }
            };

            _productServiceMock.Setup(s => s.GetByIdAsync(dto.Id))
                .ReturnsAsync(new ResultModel<Product> { Data = existingProduct });
            _categoryServiceMock.Setup(s => s.GetByIdAsync(category.Id))
                .ReturnsAsync(new ResultModel<Category> { Data = category });
            _productServiceMock.Setup(s => s.UpdateAsync(It.IsAny<Product>()))
                .ReturnsAsync(new ResultModel<Product> { Data = existingProduct });

            // Act
            var result = await _controller.Update(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<ProductResponseDto>>(okResult.Value);
            Assert.NotNull(apiResponse.Data);
            Assert.Empty(apiResponse.Errors);
            Assert.Equal($"Product '{dto.Name}' is geüpdatet.", apiResponse.Message);
        }
        [Fact]
        public async Task Update_WhenModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");
            var dto = new ProductUpdateRequestDto();

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
        public async Task Update_WhenNoCategories_ReturnsBadRequest()
        {
            // Arrange
            var dto = new ProductUpdateRequestDto { Id = Guid.NewGuid(), CategoryIds = new List<Guid>() };

            // Act
            var result = await _controller.Update(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal("Minstens één categorie is vereist.", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenProductNotFound_ReturnsNotFound()
        {
            // Arrange
            var dto = new ProductUpdateRequestDto { Id = Guid.NewGuid(), CategoryIds = new List<Guid> { Guid.NewGuid() } };
            _productServiceMock.Setup(s => s.GetByIdAsync(dto.Id))
                .ReturnsAsync(new ResultModel<Product> { Data = null, Errors = new List<string> { "Not found" } });

            // Act
            var result = await _controller.Update(dto);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(notFound.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Not found", apiResponse.Errors);
            Assert.Equal("Product werd niet gevonden.", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenCategoryNotFound_ReturnsBadRequest()
        {
            // Arrange
            var existingProduct = new Product("Burger", 10);
            var category = new Category("Burgers");

            var dto = new ProductUpdateRequestDto
            {
                Id = Guid.NewGuid(),
                CategoryIds = new List<Guid> { category.Id }
            };

            _productServiceMock.Setup(s => s.GetByIdAsync(dto.Id))
                .ReturnsAsync(new ResultModel<Product> { Data = existingProduct });
            _categoryServiceMock.Setup(s => s.GetByIdAsync(category.Id))
                .ReturnsAsync(new ResultModel<Category> { Data = null, Errors = new List<string> { "Not found" } });

            // Act
            var result = await _controller.Update(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Not found", apiResponse.Errors);
            Assert.Equal("Categorie werd niet gevonden.", apiResponse.Message);
        }
        [Fact]
        public async Task Update_WhenProductServiceReturnsErrors_ReturnsBadRequest()
        {
            // Arrange
            var existingProduct = new Product("Burger", 10);
            var category = new Category("Burgers");

            var dto = new ProductUpdateRequestDto
            {
                Id = existingProduct.Id,
                Name = "Nieuwe naam",
                Price = 15,
                Image = "afbeelding.jpg",
                CategoryIds = new List<Guid> { category.Id }
            };

            _productServiceMock.Setup(s => s.GetByIdAsync(dto.Id))
                .ReturnsAsync(new ResultModel<Product> { Data = existingProduct });

            _categoryServiceMock.Setup(s => s.GetByIdAsync(category.Id))
                .ReturnsAsync(new ResultModel<Category> { Data = category });

            _productServiceMock.Setup(s => s.UpdateAsync(It.IsAny<Product>()))
                .ReturnsAsync(new ResultModel<Product> { Errors = new List<string> { "Service error" } });

            // Act
            var result = await _controller.Update(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Service error", apiResponse.Errors);
            Assert.Equal("Updaten van product is niet gelukt.", apiResponse.Message);
        }

        
        #endregion

        #region Delete
        [Fact]
        public async Task Delete_WhenProductDeleted_ReturnsOk()
        {
            // Arrange
            var product = new Product("Burger", 10);

            _productServiceMock.Setup(s => s.GetByIdAsync(product.Id))
                .ReturnsAsync(new ResultModel<Product> { Data = product });

            _productServiceMock.Setup(s => s.DeleteAsync(product))
                .ReturnsAsync(new ResultModel<Product>());

            // Act
            var result = await _controller.Delete(product.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal($"Product '{product.Name}' is verwijderd", apiResponse.Message);
            Assert.Empty(apiResponse.Errors);
        }

        [Fact]
        public async Task Delete_WhenProductNotFound_ReturnsNotFound()
        {
            // Arrange
            var productId = Guid.NewGuid();

            _productServiceMock.Setup(s => s.GetByIdAsync(productId))
                .ReturnsAsync(new ResultModel<Product> { Data = null, Errors = new List<string> { "Not found" } });

            // Act
            var result = await _controller.Delete(productId);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(notFound.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Not found", apiResponse.Errors);
            Assert.Equal("Product werd niet gevonden.", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenProductServiceReturnsErrors_ReturnsBadRequest()
        {
            // Arrange
            var product = new Product("Burger", 10);

            _productServiceMock.Setup(s => s.GetByIdAsync(product.Id))
                .ReturnsAsync(new ResultModel<Product> { Data = product });

            _productServiceMock.Setup(s => s.DeleteAsync(product))
                .ReturnsAsync(new ResultModel<Product> { Errors = new List<string> { "Service error" } });

            // Act
            var result = await _controller.Delete(product.Id);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Service error", apiResponse.Errors);
            Assert.Equal("Verwijderen van product is niet gelukt.", apiResponse.Message);
        }
        #endregion
    }
}
