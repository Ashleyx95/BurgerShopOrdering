using BurgerShopApiConsumer.Orders;
using BurgerShopOrdering.Core.Models;
using BurgerShopOrdering.Core.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.Core.Services.Web
{
    public class OrderService : IOrderService
    {
        public int CalculateTotalItemsInOrder(Order order)
        {
            int totalItems = 0;

            if (order.OrderItems.Count == 0)
            {
                return 0;
            }

            foreach (var item in order.OrderItems)
            {
                totalItems += item.Quantity;
            }

            return totalItems;
        }
        public decimal CalculateTotalPriceOrder(Order order)
        {
            decimal totalPrice = 0;

            if (order.OrderItems.Count == 0)
            {
                return 0;
            }

            foreach (var item in order.OrderItems)
            {
                totalPrice += (item.Quantity * item.ProductPrice);
            }

            return totalPrice;
        }
        public decimal CalculateTotalPriceOrderItem(decimal price, int quantity)
        {
            return price * quantity;
        }
        public void StoreOrderInStorage(Order order)
        {
            Preferences.Set("order", JsonConvert.SerializeObject(order));
        }
        public void RemoveOrderFromStorage(Order order)
        {
            Preferences.Remove("order");
        }
        public Order GetOrderFromStorage()
        {
            var orderJson = Preferences.Get("order", string.Empty);
            var order = JsonConvert.DeserializeObject<Order>(orderJson);

            if (order == null)
            {
                return new Order { OrderItems = new List<OrderItem>() };
            }

            return order;
        }
    }
}
