using BurgerShopOrdering.core.Data;
using BurgerShopOrdering.core.Entities;
using BurgerShopOrdering.core.Services.Interfaces;
using BurgerShopOrdering.core.Services.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.core.Services
{
    public class ProductService(BurgerShopDbContext burgerDbContext) : IProductService<Product>
    {
        private BurgerShopDbContext _burgerDbContext = burgerDbContext;

        public async Task<ResultModel<IEnumerable<Product>>> GetAllAsync()
        {
            var products = await _burgerDbContext.Products
                .Include(p => p.Categories)
                .Include(p => p.OrderItems)
                .ThenInclude(oi => oi.Order)
                .ToListAsync();

            var resultModel = new ResultModel<IEnumerable<Product>>();

            if (!products.Any())
            {
                resultModel.Errors.Add("Er zijn geen beschikbare producten");
            }
            else
            {
                resultModel.Data = products;
            }

            return resultModel;
        }
        public async Task<ResultModel<IEnumerable<Product>>> GetAllVisibleProducts()
        {
            var visibleProducts = await _burgerDbContext.Products
                .Include(p => p.Categories)
                .Include(p => p.OrderItems)
                .ThenInclude(oi => oi.Order)
                .Where(p => p.IsVisible)
                .ToListAsync();

            var resultModel = new ResultModel<IEnumerable<Product>>();

            if (visibleProducts.Any())
            {
                resultModel.Data = visibleProducts;
            }
            else
            {
                resultModel.Errors.Add("Er zijn geen beschikbare producten");
            }

            return resultModel;
        }
        public async Task<ResultModel<Product>> GetByIdAsync(Guid id)
        {
            var product = await _burgerDbContext.Products
                .Include(p => p.Categories)
                .Include(p => p.OrderItems)
                .ThenInclude(oi => oi.Order)
                .FirstOrDefaultAsync(p => p.Id == id);

            var resultModel = new ResultModel<Product>();

            if (product == null)
            {
                resultModel.Errors.Add("Er is geen product met dit id");
            }
            else
            {
                resultModel.Data = product;
            }

            return resultModel;
        }
        public async Task<ResultModel<Product>> AddAsync(Product entity)
        {
            var resultModel = new ResultModel<Product>();

            if (await DoesProductNameExists(entity))
            {
                if (await IsProductVisible(entity))
                {
                    resultModel.Errors.Add($"Er bestaat al een product met de naam {entity.Name}");
                }
                else
                {
                    var product = await _burgerDbContext.Products.FirstAsync(p => p.Name == entity.Name);
                    product.IsVisible = true;

                    _burgerDbContext.Products.Update(product);
                    await _burgerDbContext.SaveChangesAsync();
                    resultModel.Data = product;
                }
            }
            else
            {
                _burgerDbContext.Products.Add(entity);
                await _burgerDbContext.SaveChangesAsync();
                resultModel.Data = entity;
            }

            return resultModel;
        }
        public async Task<ResultModel<Product>> DeleteAsync(Product entity)
        {
            var resultModel = new ResultModel<Product>();

            if (await ProductInOrderItems(entity))
            {
                entity.IsVisible = false;
            }
            else
            {
                _burgerDbContext.Products.Remove(entity);
            }

            await _burgerDbContext.SaveChangesAsync();
            resultModel.Data = entity;

            return resultModel;
        }
        public async Task<ResultModel<Product>> UpdateAsync(Product entity)
        {
            var resultModel = new ResultModel<Product>();

            _burgerDbContext.Products.Update(entity);
            await _burgerDbContext.SaveChangesAsync();

            resultModel.Data = entity;

            return resultModel;
        }
        private async Task<bool> DoesProductNameExists(Product entity)
        {
            bool productExists = await _burgerDbContext.Products
                .AnyAsync(p => p.Name == entity.Name);

            return productExists;
        }
        private async Task<bool> IsProductVisible(Product entity)
        {
            var product = await _burgerDbContext.Products
                .FirstOrDefaultAsync(p => p.Name == entity.Name);

            if (product == null)
                return false;

            return product.IsVisible;
        }
        private async Task<bool> ProductInOrderItems(Product entity)
        {
            var productsInOrderItems = await _burgerDbContext.OrderItems.AnyAsync(oi => oi.ProductId == entity.Id);

            return productsInOrderItems;
        }
    }
}
