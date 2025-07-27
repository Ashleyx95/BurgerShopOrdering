using BurgerShopApiConsumer.Categories;
using BurgerShopApiConsumer.Categories.Models;
using BurgerShopApiConsumer.Products;
using BurgerShopApiConsumer.Products.Model;
using BurgerShopOrdering.Core.Models;
using BurgerShopOrdering.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.Core.Services.Web
{
    public class MenuService : IMenuService
    {
        private readonly IProductApiService _productApiService;
        private readonly ICategoryApiService _categoryApiService;
        private readonly IAccountService _accountService;

        public MenuService(IProductApiService productApiService, ICategoryApiService categoryApiService, IAccountService accountService)
        {
            _productApiService = productApiService;
            _categoryApiService = categoryApiService;
            _accountService = accountService;
        }

        public async Task<ICollection<Category>> GetCategoriesAsync()
        {
            var result = await _categoryApiService.GetCategoriesAsync();

            if (result.Data == null || !result.Success)
            {
                return [];
            }

            var categories = new List<Category>();

            foreach (var c in result.Data)
            {
                var category = new Category
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsSelected = false
                };

                categories.Add(category);
            }

            return categories;
        }
        public async Task<ICollection<Product>> GetProductsAsync()
        {
            var result = await _productApiService.GetProductsAsync();

            if (result.Data == null || !result.Success)
            {
                return [];
            }

            var categories = await GetCategoriesAsync();
            var products = new List<Product>();

            foreach (var p in result.Data.OrderBy(p => p.Price))
            {
                var product = new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Image = p.Image,
                    ImageSource = LoadImage(p.Image),
                    Categories = new List<Category>()
                };

                foreach (var c in p.Categories)
                {
                    var category = categories.FirstOrDefault(category => category.Name == c);

                    if (category != null)
                    {
                        product.Categories.Add(category);
                    }
                }
                products.Add(product);
            }

            return products;
        }
        public async Task<ICollection<Product>> GetProductsByCategoryAsync(string categoryName)
        {
            var products = await GetProductsAsync();

            return products
                .Where(p => p.Categories.Any(c => c.Name == categoryName))
                .OrderBy(p => p.Price)
                .ToList();
        }
        public async Task<ResultModel> AddCategory(string categoryName)
        {
            var newCategory = new CategoryCreateRequestApiModel
            {
                Name = categoryName,
            };

            var result = await _categoryApiService.CreateCategoryAsync(newCategory, await _accountService.GetTokenAsync());

            return new ResultModel { Success = result.Success, Message = result.Message, Errors = result.Errors };

        }
        public async Task<ResultModel> AddNewProductToMenu(Product product)
        {
            var newProduct = new ProductCreateRequestApiModel
            {
                Name = product.Name,
                Price = product.Price,
                Image = string.IsNullOrEmpty(product.Image) ? null : product.Image,
                CategoryIds = product.Categories.Select(c => c.Id).ToArray()
            };

            var result = await _productApiService.CreateProductAsync(newProduct, await _accountService.GetTokenAsync());

            return new ResultModel { Success = result.Success, Message = result.Message, Errors = result.Errors };
        }
        public async Task<ResultModel> RemoveCategory(Category category)
        {
            var result = await _categoryApiService.DeleteCategoryAsync(category.Id, await _accountService.GetTokenAsync());

            return new ResultModel { Success = result.Success, Message = result.Message, Errors = result.Errors };
        }
        public async Task<ResultModel> RemoveProductFromMenu(Product product)
        {
            var result = await _productApiService.DeleteProductAsync(product.Id, await _accountService.GetTokenAsync());

            return new ResultModel { Success = result.Success, Message = result.Message, Errors = result.Errors };
        }
        public async Task<ResultModel> UpdateProductToMenu(Product product)
        {

            var productToUpdate = new ProductUpdateRequestApiModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Image = string.IsNullOrEmpty(product.Image) ? null : product.Image,
                CategoryIds = product.Categories.Select(c => c.Id).ToList()
            };

            var result = await _productApiService.UpdateProductAsync(productToUpdate, await _accountService.GetTokenAsync());

            return new ResultModel { Success = result.Success, Message = result.Message, Errors = result.Errors };
        }
        public string MakeFileNameSafe(string fileName)
        {
            fileName = fileName.ToLower();

            var invalidChars = Path.GetInvalidFileNameChars();
            var safeFileName = new string(fileName.Select(ch => invalidChars.Contains(ch) || ch == ' ' ? '_' : ch).ToArray());

            if (safeFileName.Length > 255)
            {
                safeFileName = safeFileName.Substring(0, 255);
            }

            return safeFileName;
        }
        private ImageSource LoadImage(string fileName)
        {
            if (fileName == "defaultproduct.jpg")
            {
                return ImageSource.FromFile(fileName);
            }

            var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            if (File.Exists(filePath))
            {
                return ImageSource.FromFile(filePath);
            }

            return ImageSource.FromFile("defaultproduct.jpg");
        }
    }
}
