using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace BurgerShopOrdering.core.Entities
{
    public class Product(Guid id, string name, decimal price, bool isVisible = true, string image = "defaultproduct.jpg")
    {
        public Guid Id { get; set; } = id;
        public string Name { get; set; } = name;
        public decimal Price { get; set; } = price;
        public ICollection<Category> Categories { get; set; } = [];
        public ICollection<OrderItem> OrderItems { get; set; } = [];
        public string Image { get; set; } = image;
        public bool IsVisible { get; set; } = isVisible;
    }
}
