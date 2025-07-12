using BurgerShopOrdering.core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.core.Services.Interfaces
{
    public interface ICrudService<T> where T : class
    {
        Task<ResultModel<IEnumerable<T>>> GetAllAsync();
        Task<ResultModel<T>> GetByIdAsync(Guid id);
        Task<ResultModel<T>> UpdateAsync(T entity);
        Task<ResultModel<T>> AddAsync(T entity);
        Task<ResultModel<T>> DeleteAsync(T entity);
    }
}
