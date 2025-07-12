using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopApiConsumer.Orders.Model
{
    public class OrderCreateRequestApiModel
    {
        public ICollection<OrderItemCreateRequestApiModel> OrderItems { get; set; }
        public decimal TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
        public OrderStatus Status { get; set; }
    }
}
