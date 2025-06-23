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
    public class OrderService(BurgerShopDbContext burgerDbContext) : IOrderService<Order>
    {
        private BurgerShopDbContext _burgerDbContext = burgerDbContext;

        public async Task<ResultModel<IEnumerable<Order>>> GetAllAsync()
        {
            var orders = await _burgerDbContext.Orders
                .Include(o => o.ApplicationUser)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(oi => oi.Categories)
                .ToListAsync();

            var resultModel = new ResultModel<IEnumerable<Order>>();

            if (orders.Any())
            {
                resultModel.Data = orders;
            }
            else
            {
                resultModel.Errors.Add("Er zijn geen beschikbare bestellingen");
            }

            return resultModel;
        }
        public async Task<ResultModel<Order>> GetByIdAsync(Guid id)
        {
            var order = await _burgerDbContext.Orders
                .Include(o => o.ApplicationUser)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(oi => oi.Categories)
                .FirstOrDefaultAsync(o => o.Id == id);

            var resultModel = new ResultModel<Order>();

            if (order == null)
            {
                resultModel.Errors.Add("Er bestaat geen bestelling met dit id");
            }
            else
            {
                resultModel.Data = order;
            }

            return resultModel;
        }
        public async Task<ResultModel<IEnumerable<Order>>> GetByStatusAsync(OrderStatus status)
        {
            var orders = await _burgerDbContext.Orders
                .Include(o => o.ApplicationUser)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(oi => oi.Categories)
                .Where(o => o.Status == status)
                .ToListAsync();

            var resultModel = new ResultModel<IEnumerable<Order>>();

            if (orders.Any())
            {
                resultModel.Data = orders;
            }
            else
            {
                resultModel.Errors.Add($"Er zijn geen bestellingen met status '{status}'");
            }

            return resultModel;
        }

        public async Task<ResultModel<IEnumerable<Order>>> GetOrdersByUserAsync(string userId)
        {
            var orders = await _burgerDbContext.Orders
                .Include(o => o.ApplicationUser)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(oi => oi.Categories)
                .Where(o => o.ApplicationUser.Id == userId)
                .ToListAsync();

            var resultModel = new ResultModel<IEnumerable<Order>>();

            if (orders.Any())
            {
                resultModel.Data = orders;
            }
            else
            {
                resultModel.Errors.Add($"Er zijn geen bestellingen van deze gebruiker");
            }

            return resultModel;
        }

        public async Task<ResultModel<IEnumerable<Order>>> GetOrdersByUserAndStatusAsync(string userId, OrderStatus status)
        {
            var orders = await _burgerDbContext.Orders
                .Include(o => o.ApplicationUser)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(oi => oi.Categories)
                .Where(o => o.ApplicationUser.Id == userId && o.Status == status)
                .ToListAsync();

            var resultModel = new ResultModel<IEnumerable<Order>>();

            if (orders.Any())
            {
                resultModel.Data = orders;
            }
            else
            {
                resultModel.Errors.Add($"Er zijn geen bestellingen van deze gebruiker met status {status}");
            }

            return resultModel;
        }
        public async Task<ResultModel<Order>> AddAsync(Order entity)
        {
            var resultModel = new ResultModel<Order>();

            _burgerDbContext.Orders.Add(entity);
            await _burgerDbContext.SaveChangesAsync();
            resultModel.Data = entity;

            return resultModel;
        }
        public async Task<ResultModel<Order>> DeleteAsync(Order entity)
        {
            var resultModel = new ResultModel<Order>();

            if (await OrderItemsInOrder(entity))
            {
                resultModel.Errors.Add("Gelieve eerst de order items van deze bestelling te verwijderen");
            }
            else
            {
                _burgerDbContext.Orders.Remove(entity);
                await _burgerDbContext.SaveChangesAsync();
                resultModel.Data = entity;
            }

            return resultModel;
        }
        public async Task<ResultModel<Order>> UpdateAsync(Order entity)
        {
            var resultModel = new ResultModel<Order>();

            _burgerDbContext.Orders.Update(entity);
            await _burgerDbContext.SaveChangesAsync();

            resultModel.Data = entity;

            return resultModel;
        }
        private async Task<bool> OrderItemsInOrder(Order entity)
        {
            var orderItemsInOrder = await _burgerDbContext.OrderItems.AnyAsync(oi => oi.OrderId == entity.Id);

            return orderItemsInOrder;
        }
    }
}
