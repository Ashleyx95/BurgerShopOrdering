using BurgerShopApiConsumer.Categories.Models;
using BurgerShopApiConsumer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopApiConsumer.Categories
{
    public interface ICategoryApiService
    {
        Task<ApiResponse<CategoryResponseApiModel[]>> GetCategoriesAsync();
        Task<ApiResponse<CategoryResponseApiModel>> GetCategoryByIdAsync(Guid id);
        Task<ApiResponse<object>> CreateCategoryAsync(CategoryCreateRequestApiModel categoryToCreate, string token);
        Task<ApiResponse<object>> UpdateCategoryAsync(CategoryUpdateRequestApiModel categoryToUpdate, string token);
        Task<ApiResponse<object>> DeleteCategoryAsync(Guid id, string token);
    }
}
