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
    public class OrderItemService(BurgerShopDbContext burgerDbContext) : ICrudService<OrderItem>
    {
        private BurgerShopDbContext _burgerDbContext = burgerDbContext;

        public async Task<ResultModel<IEnumerable<OrderItem>>> GetAllAsync()
        {
            var orderItems = await _burgerDbContext.OrderItems
                .Include(oi => oi.Order)
                .ThenInclude(o => o.ApplicationUser)
                .Include(o => o.Product)
                .ThenInclude(p => p.Categories)
                .ToListAsync();

            var resultModel = new ResultModel<IEnumerable<OrderItem>>();

            if (orderItems.Any())
            {
                resultModel.Data = orderItems;
            }
            else
            {
                resultModel.Errors.Add("Er zijn geen beschikbare order items");
            }

            return resultModel;
        }
        public async Task<ResultModel<OrderItem>> GetByIdAsync(Guid id)
        {
            var orderItem = await _burgerDbContext.OrderItems
                .Include(oi => oi.Order)
                .ThenInclude(o => o.ApplicationUser)
                .Include(o => o.Product)
                .ThenInclude(p => p.Categories)
                .FirstOrDefaultAsync(oi => oi.Id == id);

            var resultModel = new ResultModel<OrderItem>();

            if (orderItem == null)
            {
                resultModel.Errors.Add("Er bestaat geen order item met dit id");
            }
            else
            {
                resultModel.Data = orderItem;
            }

            return resultModel;
        }
        public async Task<ResultModel<OrderItem>> AddAsync(OrderItem entity)
        {
            var resultModel = new ResultModel<OrderItem>();

            _burgerDbContext.OrderItems.Add(entity);
            await _burgerDbContext.SaveChangesAsync();
            resultModel.Data = entity;

            return resultModel;
        }
        public async Task<ResultModel<OrderItem>> DeleteAsync(OrderItem entity)
        {
            var resultModel = new ResultModel<OrderItem>();

            _burgerDbContext.OrderItems.Remove(entity);
            await _burgerDbContext.SaveChangesAsync();
            resultModel.Data = entity;

            return resultModel;
        }
        public async Task<ResultModel<OrderItem>> UpdateAsync(OrderItem entity)
        {
            var resultModel = new ResultModel<OrderItem>();

            _burgerDbContext.OrderItems.Update(entity);
            await _burgerDbContext.SaveChangesAsync();

            resultModel.Data = entity;

            return resultModel;
        }
    }
}
