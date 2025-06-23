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
    public class CategoryService(BurgerShopDbContext burgerDbContext) : ICategoryService<Category>
    {
        private BurgerShopDbContext _burgerDbContext = burgerDbContext;

        public async Task<ResultModel<IEnumerable<Category>>> GetAllAsync()
        {
            var categories = await _burgerDbContext.Categories
                .Include(c => c.Products)
                .ToListAsync();

            var resultModel = new ResultModel<IEnumerable<Category>>();

            if (categories.Any())
            {
                resultModel.Data = categories;
            }
            else
            {
                resultModel.Errors.Add("Er zijn geen beschikbare categorieën");
            }

            return resultModel;
        }
        public async Task<ResultModel<IEnumerable<Category>>> GetAllVisibleCategories()
        {
            var visibleCategories = await _burgerDbContext.Categories
                .Include(c => c.Products)
                .Where(c => c.IsVisible)
                .ToListAsync();

            var resultModel = new ResultModel<IEnumerable<Category>>();

            if (visibleCategories.Any())
            {
                resultModel.Data = visibleCategories;
            }
            else
            {
                resultModel.Errors.Add("Er zijn geen beschikbare categorieën");
            }

            return resultModel;
        }
        public async Task<ResultModel<Category>> GetByIdAsync(Guid id)
        {
            var category = await _burgerDbContext.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            var resultModel = new ResultModel<Category>();

            if (category == null)
            {
                resultModel.Errors.Add("Er is geen categorie met dit id");
            }
            else
            {
                resultModel.Data = category;
            }

            return resultModel;
        }
        public async Task<ResultModel<Category>> AddAsync(Category entity)
        {
            var resultModel = new ResultModel<Category>();

            if (await DoesCategoryNameExists(entity))
            {
                if (await IsCategoryVisible(entity))
                {
                    resultModel.Errors.Add($"Er bestaat al een categorie met de naam {entity.Name}");
                }
                else
                {
                    var category = await _burgerDbContext.Categories.FirstAsync(c => c.Name == entity.Name);
                    category.IsVisible = true;

                    _burgerDbContext.Categories.Update(category);
                    await _burgerDbContext.SaveChangesAsync();
                    resultModel.Data = category;
                }
            }
            else
            {
                _burgerDbContext.Categories.Add(entity);
                await _burgerDbContext.SaveChangesAsync();
                resultModel.Data = entity;
            }

            return resultModel;
        }
        public async Task<ResultModel<Category>> DeleteAsync(Category entity)
        {
            var resultModel = new ResultModel<Category>();

            if (await ProductsInCategory(entity))
            {
                if (await CategoryHasVisibleProducts(entity))
                {
                    resultModel.Errors.Add("Er bevinden zich nog producten in deze categorie, verwijder deze eerst");
                    return resultModel;
                }
                else
                {
                    entity.IsVisible = false;
                }
            }
            else
            {
                _burgerDbContext.Categories.Remove(entity);
            }

            await _burgerDbContext.SaveChangesAsync();
            resultModel.Data = entity;

            return resultModel;
        }
        public async Task<ResultModel<Category>> UpdateAsync(Category entity)
        {
            var resultModel = new ResultModel<Category>();

            _burgerDbContext.Categories.Update(entity);
            await _burgerDbContext.SaveChangesAsync();

            resultModel.Data = entity;

            return resultModel;
        }
        private async Task<bool> DoesCategoryNameExists(Category entity)
        {
            bool categoryExists = await _burgerDbContext.Categories
                .AnyAsync(c => c.Name == entity.Name);

            return categoryExists;
        }
        private async Task<bool> IsCategoryVisible(Category entity)
        {
            var category = await _burgerDbContext.Categories
                .FirstOrDefaultAsync(c => c.Name == entity.Name);

            if (category == null)
                return false;

            return category.IsVisible;
        }
        private async Task<bool> ProductsInCategory(Category entity)
        {
            var productsInCategory = await _burgerDbContext.Products.AnyAsync(p => p.Categories.Any(c => c.Id == entity.Id));

            return productsInCategory;
        }

        private async Task<bool> CategoryHasVisibleProducts(Category entity)
        {
            var visibleProductsInCategory = await _burgerDbContext.Products.AnyAsync(p => p.Categories.Any(c => c.Id == entity.Id) && p.IsVisible);

            return visibleProductsInCategory;
        }
    }
}
