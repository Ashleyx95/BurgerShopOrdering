using BurgerShopOrdering.api.Controllers;
using BurgerShopOrdering.api.Dtos.Categories;
using BurgerShopOrdering.api.Dtos.Common;
using BurgerShopOrdering.core.Entities;
using BurgerShopOrdering.core.Services.Interfaces;
using BurgerShopOrdering.core.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.test.API
{
    public class CategoriesControllerTests
    {
        private readonly Mock<ICategoryService<Category>> _categoryServiceMock;
        private readonly CategoriesController _controller;

        public CategoriesControllerTests()
        {
            _categoryServiceMock = new Mock<ICategoryService<Category>>();
            _controller = new CategoriesController(_categoryServiceMock.Object);
        }

        #region Get
        [Fact]
        public async Task Get_WhenCategoriesExist_ReturnsOk()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category("Burgers")
            };

            _categoryServiceMock.Setup(s => s.GetAllVisibleCategories())
                .ReturnsAsync(new ResultModel<IEnumerable<Category>> { Data = categories });

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<List<CategoryResponseDto>>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Single(apiResponse.Data);
            Assert.Empty(apiResponse.Errors);
            Assert.Equal("Burgers", apiResponse.Data[0].Name);
            Assert.Equal("Categorieën opgehaald.", apiResponse.Message);
        }

        [Fact]
        public async Task Get_WhenCategoryServiceReturnsErrors_ReturnsBadRequest()
        {
            // Arrange
            _categoryServiceMock.Setup(s => s.GetAllVisibleCategories())
                .ReturnsAsync(new ResultModel<IEnumerable<Category>> { Errors = new List<string> { "Service error" } });

            // Act
            var result = await _controller.Get();

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Service error", apiResponse.Errors);
            Assert.Equal("Categorieën konden niet worden opgehaald.", apiResponse.Message);
        }

        [Fact]
        public async Task Get_ById_WhenCategoryFound_ReturnsOk()
        {
            // Arrange
            var category = new Category("Burgers");

            _categoryServiceMock.Setup(s => s.GetByIdAsync(category.Id))
                .ReturnsAsync(new ResultModel<Category> { Data = category });

            // Act
            var result = await _controller.Get(category.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<CategoryResponseDto>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Empty(apiResponse.Errors);
            Assert.Equal("Burgers", apiResponse.Data.Name);
            Assert.Equal("Categorie werd opgehaald.", apiResponse.Message);
        }

        [Fact]
        public async Task Get_ById_WhenCategoryNotFound_ReturnsNotFound()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            _categoryServiceMock.Setup(s => s.GetByIdAsync(categoryId))
                .ReturnsAsync(new ResultModel<Category> { Data = null, Errors = new List<string> { "Not found" } });

            // Act
            var result = await _controller.Get(categoryId);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(notFound.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Not found", apiResponse.Errors);
            Assert.Equal("Categorie werd niet gevonden.", apiResponse.Message);
        }
        #endregion

        #region Add
        [Fact]
        public async Task Add_WhenCategoryAdded_ReturnsCreatedAtAction()
        {
            // Arrange
            var dto = new CategoryCreateRequestDto { Name = "Burgers" };

            var category = new Category(dto.Name);

            _categoryServiceMock.Setup(s => s.AddAsync(It.IsAny<Category>()))
                .ReturnsAsync(new ResultModel<Category> { Data = category });

            // Act
            var result = await _controller.Add(dto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<CategoryResponseDto>>(createdResult.Value);
            Assert.NotNull(apiResponse.Data);
            Assert.True(apiResponse.Success);
            Assert.Empty(apiResponse.Errors);
            Assert.Equal($"Categorie '{dto.Name}' is toegevoegd", apiResponse.Message);
        }

        [Fact]
        public async Task Add_WhenModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");
            var dto = new CategoryCreateRequestDto();

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
        public async Task Add_WhenCategoryServiceReturnsErrors_ReturnsBadRequest()
        {
            // Arrange
            var dto = new CategoryCreateRequestDto { Name = "Burgers" };

            _categoryServiceMock.Setup(s => s.AddAsync(It.IsAny<Category>()))
                .ReturnsAsync(new ResultModel<Category> { Errors = new List<string> { "Service error" } });

            // Act
            var result = await _controller.Add(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Service error", apiResponse.Errors);
            Assert.Equal("Categorie kon niet worden toegevoegd.", apiResponse.Message);
        }
        #endregion

        #region Update
        [Fact]
        public async Task Update_WhenCategoryUpdated_ReturnsOk()
        {
            // Arrange
            var existingCategory = new Category("Old name");
            var dto = new CategoryUpdateRequestDto {  Id = existingCategory.Id, Name = "New name" };

            _categoryServiceMock.Setup(s => s.GetByIdAsync(dto.Id))
                .ReturnsAsync(new ResultModel<Category> { Data = existingCategory });
            _categoryServiceMock.Setup(s => s.UpdateAsync(It.IsAny<Category>()))
                .ReturnsAsync(new ResultModel<Category> { Data = existingCategory });

            // Act
            var result = await _controller.Update(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<CategoryResponseDto>>(okResult.Value);
            Assert.NotNull(apiResponse.Data);
            Assert.True(apiResponse.Success);
            Assert.Empty(apiResponse.Errors);
            Assert.Equal($"Categorie '{dto.Name}' is geüpdatet", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");
            var dto = new CategoryUpdateRequestDto();

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
        public async Task Update_WhenCategoryNotFound_ReturnsNotFound()
        {
            // Arrange
            var dto = new CategoryUpdateRequestDto { Id = Guid.NewGuid() , Name = "New name" };

            _categoryServiceMock.Setup(s => s.GetByIdAsync(dto.Id))
                .ReturnsAsync(new ResultModel<Category> { Data = null, Errors = new List<string> { "Not found" } });

            // Act
            var result = await _controller.Update(dto);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(notFound.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Not found", apiResponse.Errors);
            Assert.Equal("Categorie werd niet gevonden.", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenCategoryServiceReturnsErrors_ReturnsBadRequest()
        {
            // Arrange
            var existingCategory = new Category("Old name");
            var dto = new CategoryUpdateRequestDto { Id = existingCategory.Id, Name = "New name" };

            _categoryServiceMock.Setup(s => s.GetByIdAsync(dto.Id))
                .ReturnsAsync(new ResultModel<Category> { Data = existingCategory });
            _categoryServiceMock.Setup(s => s.UpdateAsync(It.IsAny<Category>()))
                .ReturnsAsync(new ResultModel<Category> { Errors = new List<string> { "Service error" } });

            // Act
            var result = await _controller.Update(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Service error", apiResponse.Errors);
            Assert.Equal("Updaten van categorie is niet gelukt.", apiResponse.Message);
        }
        #endregion

        #region Delete
        [Fact]
        public async Task Delete_WhenCategoryDeleted_ReturnsOk()
        {
            // Arrange
            var category = new Category("Burgers");

            _categoryServiceMock.Setup(s => s.GetByIdAsync(category.Id))
                .ReturnsAsync(new ResultModel<Category> { Data = category });
            _categoryServiceMock.Setup(s => s.DeleteAsync(category))
                .ReturnsAsync(new ResultModel<Category>());

            // Act
            var result = await _controller.Delete(category.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Empty(apiResponse.Errors);
            Assert.Equal($"Categorie '{category.Name}' is verwijderd", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenCategoryNotFound_ReturnsNotFound()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            _categoryServiceMock.Setup(s => s.GetByIdAsync(categoryId))
                .ReturnsAsync(new ResultModel<Category> { Data = null, Errors = new List<string> { "Not found" } });

            // Act
            var result = await _controller.Delete(categoryId);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(notFound.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Not found", apiResponse.Errors);
            Assert.Equal("Categorie werd niet gevonden.", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenCategoryServiceReturnsErrors_ReturnsBadRequest()
        {
            // Arrange
            var category = new Category("Burgers");

            _categoryServiceMock.Setup(s => s.GetByIdAsync(category.Id))
                .ReturnsAsync(new ResultModel<Category> { Data = category });
            _categoryServiceMock.Setup(s => s.DeleteAsync(category))
                .ReturnsAsync(new ResultModel<Category> { Errors = new List<string> { "Service error" } });

            // Act
            var result = await _controller.Delete(category.Id);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("Service error", apiResponse.Errors);
            Assert.Equal("Verwijderen van categorie is niet gelukt.", apiResponse.Message);
        }
        #endregion
    }
}
