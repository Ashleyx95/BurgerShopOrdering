using BurgerShopApiConsumer.Common;
using BurgerShopApiConsumer.Orders.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopApiConsumer.Orders
{
    public interface IOrderApiService
    {
        Task<ApiResponse<OrderResponseApiModel[]>> GetOrdersAsync(string token);
        Task<ApiResponse<OrderResponseApiModel>> GetOrderByIdAsync(Guid id, string token);
        Task<ApiResponse<OrderResponseApiModel[]>> GetOrdersByStatusAsync(string status, string token);
        Task<ApiResponse<object>> CreateOrderAsync(OrderCreateRequestApiModel orderToCreate, string token);
        Task<ApiResponse<object>> UpdateOrderAsync(OrderUpdateRequestApiModel categoryToUpdate, string token);
        Task<ApiResponse<object>> DeleteOrderAsync(Guid id, string token);
    }
}
