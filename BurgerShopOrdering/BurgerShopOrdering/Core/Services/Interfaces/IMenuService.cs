using BurgerShopOrdering.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.Core.Services.Interfaces
{
    public interface IMenuService
    {
        Task<ICollection<Product>> GetProductsAsync();
        Task<ICollection<Product>> GetProductsByCategoryAsync(string category);
        Task<ICollection<Category>> GetCategoriesAsync();
        Task<ResultModel> AddCategory(string Name);
        Task<ResultModel> RemoveCategory(Category category);
        Task<ResultModel> AddNewProductToMenu(Product product);
        Task<ResultModel> RemoveProductFromMenu(Product product);
        Task<ResultModel> UpdateProductToMenu(Product product);
        string MakeFileNameSafe(string fileName);
    }
}
