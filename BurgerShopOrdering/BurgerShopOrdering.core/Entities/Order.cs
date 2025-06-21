using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BurgerShopOrdering.core.Entities
{
    public class Order(Guid id, string applicationUserId, string name, decimal totalPrice, int quantity)
    {
        public Guid Id { get; set; } = id;
        public string ApplicationUserId { get; set; } = applicationUserId;
        public ApplicationUser ApplicationUser { get; set; }
        public string Name { get; set; } = name;
        public ICollection<OrderItem> OrderItems { get; set; } = [];
        public decimal TotalPrice { get; set; } = totalPrice;
        public int Quantity { get; set; } = quantity;
        public OrderStatus Status { get; set; } = OrderStatus.Besteld;
        public DateTime DateOrdered { get; set; } = DateTime.Now;
        public DateTime? DateDelivered { get; set; } = null;
    }
}
