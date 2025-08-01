﻿using BurgerShopOrdering.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.Core.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ICollection<Order>> GetOrdersAsync();
        Task<ICollection<Order>> GetOrdersByStatusAsync(string status);
        Task<ResultModel> CreateOrderAsync(Order order);
        Task<ResultModel> UpdateOrderStatusAsync(Order order);
        decimal CalculateTotalPriceOrderItem(decimal price, int quantity);
        decimal CalculateTotalPriceOrder(Order order);
        int CalculateTotalItemsInOrder(Order order);
        void StoreOrderInStorage(Order order);
        void RemoveOrderFromStorage(Order order);
        Order GetOrderFromStorage();
    }
}
