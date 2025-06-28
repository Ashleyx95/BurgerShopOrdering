using BurgerShopOrdering.api.Dtos.Categories;
using BurgerShopOrdering.api.Dtos.Common;
using BurgerShopOrdering.api.Dtos.Products;
using BurgerShopOrdering.core.Entities;
using BurgerShopOrdering.core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BurgerShopOrdering.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryService<Category> categoryService) : ControllerBase
    {
        private readonly ICategoryService<Category> _categoryService = categoryService;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _categoryService.GetAllVisibleCategories();

            if (result.Success && result.Data != null)
            {
                var categoryResponseDtos = new List<CategoryResponseDto>();

                foreach (var category in result.Data)
                {
                    var categoryResponseDto = new CategoryResponseDto
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Products = category.Products.Select(p => new ProductResponseDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Price = p.Price,
                            Image = p.Image,
                            Categories = p.Categories.Select(c => c.Name).ToList(),
                        }).ToList()
                    };

                    categoryResponseDtos.Add(categoryResponseDto);
                }

                return Ok(ApiResponse<List<CategoryResponseDto>>.SuccessResponse(categoryResponseDtos, "Categorieën opgehaald."));
            }

            return BadRequest(ApiResponse<object>.FailureResponse("Categorieën konden niet worden opgehaald.", result.Errors));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _categoryService.GetByIdAsync(id);

            if (!result.Success || result.Data == null)
            {
                return NotFound(ApiResponse<object>.FailureResponse("Categorie werd niet gevonden.", result.Errors));
            }

            var category = result.Data;

            var categoryResponseDto = new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Products = category.Products.Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Image = p.Image,
                    Categories = p.Categories.Select(c => c.Name).ToList()
                }).ToList()
            };

            return Ok(ApiResponse<CategoryResponseDto>.SuccessResponse(categoryResponseDto, "Categorie werd opgehaald."));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(CategoryCreateRequestDto categoryCreateRequestDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(ApiResponse<object>.FailureResponse("Ongeldige invoer.", errors));
            }

            var category = new Category(categoryCreateRequestDto.Name);

            var result = await _categoryService.AddAsync(category);

            if (result.Success)
            {
                return CreatedAtAction(nameof(Get), new { id = category.Id }, ApiResponse<object>.SuccessResponse(null, $"Categorie '{categoryCreateRequestDto.Name}' is toegevoegd"));
            }

            return BadRequest(ApiResponse<object>.FailureResponse("Categorie kon niet worden toegevoegd.", result.Errors));
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(CategoryUpdateRequestDto categoryUpdateRequestDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(ApiResponse<object>.FailureResponse("Ongeldige invoer.", errors));
            }

            var categoryResult = await _categoryService.GetByIdAsync(categoryUpdateRequestDto.Id);

            if (!categoryResult.Success || categoryResult.Data == null)
            {
                return NotFound(ApiResponse<object>.FailureResponse("Categorie werd niet gevonden.", categoryResult.Errors));
            }

            var category = categoryResult.Data;
            category.Name = categoryUpdateRequestDto.Name;

            var result = await _categoryService.UpdateAsync(category);

            if (result.Success)
            {
                return Ok(ApiResponse<object>.SuccessResponse(null, $"Categorie '{categoryUpdateRequestDto.Name}' is geüpdatet"));
            }

            return BadRequest(ApiResponse<object>.FailureResponse("Updaten van categorie is niet gelukt.", result.Errors));
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var categoryResult = await _categoryService.GetByIdAsync(id);

            if (!categoryResult.Success || categoryResult.Data == null)
            {
                return NotFound(ApiResponse<object>.FailureResponse("Categorie werd niet gevonden.", categoryResult.Errors));
            }

            var category = categoryResult.Data;

            var result = await _categoryService.DeleteAsync(category);

            if (result.Success)
            {
                return Ok(ApiResponse<object>.SuccessResponse(null, $"Categorie '{category.Name}' is verwijderd"));
            }

            return BadRequest(ApiResponse<object>.FailureResponse("Verwijderen van categorie is niet gelukt.", result.Errors));
        }
    }
}
