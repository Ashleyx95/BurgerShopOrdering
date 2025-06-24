using BurgerShopOrdering.api.Dtos.Products;
using BurgerShopOrdering.core.Entities;
using BurgerShopOrdering.core.Services.Interfaces;
using BurgerShopOrdering.core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BurgerShopOrdering.api.Dtos.Common;
using static System.Net.Mime.MediaTypeNames;

namespace BurgerShopOrdering.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService<Product> productService, ICategoryService<Category> categoryService) : ControllerBase
    {
        private readonly IProductService<Product> _productService = productService;
        private readonly ICategoryService<Category> _categoryService = categoryService;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _productService.GetAllVisibleProducts();

            if (result.Success && result.Data != null)
            {
                var productResponseDtos = new List<ProductResponseDto>();

                foreach (var product in result.Data)
                {
                    var productResponseDto = new ProductResponseDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Image = product.Image,
                        Price = product.Price,
                        Categories = product.Categories.Select(c => c.Name).ToList(),
                    };

                    productResponseDtos.Add(productResponseDto);
                }
                return Ok(ApiResponse<List<ProductResponseDto>>.SuccessResponse(productResponseDtos, "Producten opgehaald"));
            }

            return BadRequest(ApiResponse<object>.FailureResponse("Producten konden niet worden opgehaald.", result.Errors));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _productService.GetByIdAsync(id);

            if (!result.Success || result.Data == null)
            {
                return NotFound(ApiResponse<object>.FailureResponse("Product werd niet gevonden.", result.Errors));
            }

            var product = result.Data;

            var productResponseDto = new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Image = product.Image,
                Price = product.Price,
                Categories = product.Categories.Select(c => c.Name).ToList(),
            };

            return Ok(ApiResponse<ProductResponseDto>.SuccessResponse(productResponseDto, "Product opgehaald"));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(ProductCreateRequestDto productCreateRequestDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(ApiResponse<object>.FailureResponse("Ongeldige invoer.", errors));
            }

            if (productCreateRequestDto.CategoryIds == null || !productCreateRequestDto.CategoryIds.Any())
            {
                return BadRequest(ApiResponse<object>.FailureResponse("Minstens één categorie is vereist."));
            }

            var product = new Product(productCreateRequestDto.Name, productCreateRequestDto.Price, true, productCreateRequestDto.Image ?? "default.jpg");

            foreach (var categoryId in productCreateRequestDto.CategoryIds)
            {
                var category = await _categoryService.GetByIdAsync(categoryId);

                if (!category.Success || category.Data == null || !category.Data.IsVisible)
                {
                    return BadRequest(ApiResponse<object>.FailureResponse("Categorie werd niet gevonden.", category.Errors));
                }

                product.Categories.Add(category.Data);
            }

            var result = await _productService.AddAsync(product);

            if (result.Success)
            {
                return CreatedAtAction(nameof(Get), new { id = product.Id }, ApiResponse<object>.SuccessResponse(null, $"Product '{product.Name}' is toegevoegd."));
            }

            return BadRequest(ApiResponse<object>.FailureResponse("Product kon niet worden toegevoegd.", result.Errors));
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(ProductUpdateRequestDto productUpdateRequestDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(ApiResponse<object>.FailureResponse("Ongeldige invoer.", errors));
            }

            if (productUpdateRequestDto.CategoryIds == null || !productUpdateRequestDto.CategoryIds.Any())
            {
                return BadRequest(ApiResponse<object>.FailureResponse("Minstens één categorie is vereist."));
            }

            var productResult = await _productService.GetByIdAsync(productUpdateRequestDto.Id);

            if (!productResult.Success || productResult.Data == null)
            {
                return NotFound(ApiResponse<object>.FailureResponse("Product werd niet gevonden.", productResult.Errors));
            }

            var product = productResult.Data;

            product.Name = productUpdateRequestDto.Name;
            product.Price = productUpdateRequestDto.Price;
            product.Image = productUpdateRequestDto.Image ?? "default.jpg";

            var categories = new List<Category>();

            foreach (var categoryId in productUpdateRequestDto.CategoryIds)
            {
                var categoryResult = await _categoryService.GetByIdAsync(categoryId);

                if (!categoryResult.Success || categoryResult.Data == null || !categoryResult.Data.IsVisible)
                {
                    return BadRequest(ApiResponse<object>.FailureResponse("Categorie werd niet gevonden.", categoryResult.Errors));
                }

                categories.Add(categoryResult.Data);
            }

            product.Categories = categories;

            var result = await _productService.UpdateAsync(product);

            if (result.Success)
            {
                return Ok(ApiResponse<object>.SuccessResponse(null, $"Product '{productUpdateRequestDto.Name}' is geüpdatet"));
            }

            return BadRequest(ApiResponse<object>.FailureResponse("Updaten van product is niet gelukt.", result.Errors));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var productResult = await _productService.GetByIdAsync(id);

            if (!productResult.Success || productResult.Data == null)
            {
                return NotFound(ApiResponse<object>.FailureResponse("Product werd niet gevonden.", productResult.Errors));
            }

            var product = productResult.Data;

            var result = await _productService.DeleteAsync(product);

            if (result.Success)
            {
                return Ok(ApiResponse<object>.SuccessResponse(null, $"Product '{productResult.Data.Name}' is verwijderd"));
            }

            return BadRequest(ApiResponse<object>.FailureResponse("Verwijderen van product is niet gelukt.", result.Errors));
        }
    }
}
