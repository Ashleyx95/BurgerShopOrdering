using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.Core.Models
{
    public class OrderItem
    {
        public string ProductName { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public ImageSource ImageSource { get; set; }
    }
}
