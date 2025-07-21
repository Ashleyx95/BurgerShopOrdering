using BurgerShopOrdering.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.Core.Services.Interfaces
{
    public interface IOrderService
    {
        decimal CalculateTotalPriceOrderItem(decimal price, int quantity);
        decimal CalculateTotalPriceOrder(Order order);
        int CalculateTotalItemsInOrder(Order order);
        void StoreOrderInStorage(Order order);
        void RemoveOrderFromStorage(Order order);
        Order GetOrderFromStorage();
    }
}
