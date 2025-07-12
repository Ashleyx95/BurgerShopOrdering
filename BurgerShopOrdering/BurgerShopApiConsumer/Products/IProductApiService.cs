using BurgerShopApiConsumer.Common;
using BurgerShopApiConsumer.Products.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopApiConsumer.Products
{
    public interface IProductApiService
    {
        Task<ApiResponse<ProductResponseApiModel[]>> GetProductsAsync();
        Task<ApiResponse<ProductResponseApiModel>> GetProductByIdAsync(Guid id);
        Task<ApiResponse<object>> CreateProductAsync(ProductCreateRequestApiModel productToCreate, string token);
        Task<ApiResponse<object>> UpdateProductAsync(ProductUpdateRequestApiModel productToUpdate, string token);
        Task<ApiResponse<object>> DeleteProductAsync(Guid id, string token);
    }
}
