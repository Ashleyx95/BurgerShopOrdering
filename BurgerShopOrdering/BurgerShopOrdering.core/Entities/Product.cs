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
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ICollection<Category> Categories { get; set; } = [];
        public ICollection<OrderItem> OrderItems { get; set; } = [];
        public string Image { get; set; }
        public bool IsVisible { get; set; }

        // Constructor for seeding
        public Product(Guid id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
            IsVisible = true;
            Image = "default.jpg";
        }
        public Product(string name, decimal price, bool isVisible = true, string image = "defaultproduct.jpg")
        {
            Id = Id = Guid.NewGuid();
            Name = name;
            Price = price;
            Image = image;
            IsVisible = isVisible;
        }
    }
}
