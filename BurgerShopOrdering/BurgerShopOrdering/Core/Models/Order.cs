using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.Core.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string NameUser { get; set; }
        public string Name { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public decimal TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime DateOrdered { get; set; }
        public DateTime? DateDelivered { get; set; }
    }
}
