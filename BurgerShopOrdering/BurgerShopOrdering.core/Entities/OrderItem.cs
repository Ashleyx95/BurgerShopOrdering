using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.core.Entities
{
    public class OrderItem(Guid id, Guid orderId, Guid productId, int quantity, decimal price)
    {
        public Guid Id { get; set; } = id;
        public Guid OrderId { get; set; } = orderId;
        public Order Order { get; set; }
        public Guid ProductId { get; set; } = productId;
        public Product Product { get; set; }
        public int Quantity { get; set; } = quantity;
        public decimal Price { get; set; } = price;
    }
}
