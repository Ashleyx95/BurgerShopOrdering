using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopApiConsumer.Orders.Model
{
    public class OrderUpdateRequestApiModel
    {
        public Guid Id { get; set; }
        public OrderStatus Status { get; set; }
    }
}
