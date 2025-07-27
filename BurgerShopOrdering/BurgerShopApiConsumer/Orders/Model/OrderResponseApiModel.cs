using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopApiConsumer.Orders.Model
{
    public class OrderResponseApiModel
    {
        public Guid Id { get; set; }
        public string NameUser { get; set; }
        public string ApplicationUserId { get; set; }
        public string Name { get; set; }
        public ICollection<OrderItemResponseApiModel> OrderItems { get; set; }
        public decimal TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
        public string Status { get; set; }
        public DateTime DateOrdered { get; set; }
        public DateTime? DateDelivered { get; set; }
    }
}
