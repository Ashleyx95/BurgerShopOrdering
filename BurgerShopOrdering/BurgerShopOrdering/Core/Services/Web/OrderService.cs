using BurgerShopApiConsumer.Orders;
using BurgerShopApiConsumer.Orders.Model;
using BurgerShopOrdering.Core.Models;
using BurgerShopOrdering.Core.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderStatus = BurgerShopOrdering.Core.Models.OrderStatus;

namespace BurgerShopOrdering.Core.Services.Web
{
    public class OrderService : IOrderService
    {
        private readonly IOrderApiService _orderApiService;
        private readonly IAccountService _accountService;

        public OrderService(IOrderApiService orderApiService, IAccountService accountService)
        {
            _orderApiService = orderApiService;
            _accountService = accountService;
        }
        public async Task<ICollection<Order>> GetOrdersAsync()
        {
            var result = await _orderApiService.GetOrdersAsync(await _accountService.GetTokenAsync());

            return result.Success && result.Data != null? MapOrders(result.Data) : [];
        }
        public async Task<ICollection<Order>> GetOrdersByStatusAsync(string status)
        {
            var result = await _orderApiService.GetOrdersByStatusAsync(status, await _accountService.GetTokenAsync());

            return result.Success && result.Data != null ? MapOrders(result.Data) : [];
        }
        public async Task<ResultModel> CreateOrderAsync(Order order)
        {
            var user = await _accountService.GetLoggedInUserAsync();

            var newOrder = new OrderCreateRequestApiModel
            {
                TotalPrice = order.TotalPrice,
                TotalQuantity = order.TotalQuantity,
                Status = BurgerShopApiConsumer.Orders.Model.OrderStatus.Besteld,
                OrderItems = order.OrderItems.Select(oi => new OrderItemCreateRequestApiModel
                {
                    Price = oi.ProductPrice,
                    Quantity = oi.Quantity,
                    ProductId = oi.ProductId,
                }).ToList(),
            };

            var result = await _orderApiService.CreateOrderAsync(newOrder, await _accountService.GetTokenAsync());

            return new ResultModel { Success = result.Success, Message = result.Message, Errors = result.Errors };
        }
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
        private List<Order> MapOrders(IEnumerable<OrderResponseApiModel> responseOrders)
        {
            var orders = new List<Order>();

            foreach (var o in responseOrders.OrderByDescending(o => o.DateOrdered))
            {
                var order = new Order
                {
                    Id = o.Id,
                    Name = o.Name,
                    TotalPrice = o.TotalPrice,
                    TotalQuantity = o.TotalQuantity,
                    DateOrdered = o.DateOrdered,
                    NameUser = o.NameUser,
                    Status = Enum.Parse<OrderStatus>(o.Status),
                    DateDelivered = o.DateDelivered,
                    OrderItems = o.OrderItems.Select(oi => new OrderItem
                    {
                        ProductName = oi.ProductName,
                        ProductPrice = oi.Price,
                        Quantity = oi.Quantity,
                        TotalPrice = CalculateTotalPriceOrderItem(oi.Price, oi.Quantity)
                    }).ToList()
                };

                orders.Add(order);
            }

            return orders;
        }
    }
}
