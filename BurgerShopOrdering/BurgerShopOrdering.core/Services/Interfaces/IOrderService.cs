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
        Task<ResultModel<IEnumerable<T>>> GetOrdersByUserAsync(ApplicationUser user);
        Task<ResultModel<IEnumerable<T>>> GetByStatusAsync(string status);
    }
}
