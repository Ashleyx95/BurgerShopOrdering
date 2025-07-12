using BurgerShopOrdering.core.Entities;
using BurgerShopOrdering.core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.core.Services.Interfaces
{
    public interface IOrderService<T> : ICrudService<T> where T : class
    {
        Task<ResultModel<IEnumerable<T>>> GetOrdersByUserAsync(string userId);
        Task<ResultModel<IEnumerable<T>>> GetByStatusAsync(OrderStatus status);
        Task<ResultModel<IEnumerable<T>>> GetOrdersByUserAndStatusAsync(string userId, OrderStatus status);
    }
}
