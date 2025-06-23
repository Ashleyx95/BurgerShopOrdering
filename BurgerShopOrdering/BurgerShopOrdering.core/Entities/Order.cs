using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BurgerShopOrdering.core.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string Name { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = [];
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Besteld;
        public DateTime DateOrdered { get; set; } = DateTime.Now;
        public DateTime? DateDelivered { get; set; } = null;

        //Constructor for seeding
        public Order(Guid id, string applicationUserId, string name, decimal totalPrice, int quantity)
        {
            Id = id;
            ApplicationUserId = applicationUserId;
            Name = name;
            TotalPrice = totalPrice;
            Quantity = quantity;
        }
        public Order(string applicationUserId, string name, decimal totalPrice, int quantity)
        {
            Id = Guid.NewGuid();
            ApplicationUserId = applicationUserId;
            Name = name;
            TotalPrice = totalPrice;
            Quantity = quantity;
        }
    }
}
