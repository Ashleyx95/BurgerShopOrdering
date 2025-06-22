using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BurgerShopOrdering.core.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product>? Products { get; set; } = [];
        public bool IsVisible { get; set; }

        // Constructor for seeding
        public Category(Guid id, string name, bool isVisible = true) 
        {
            Id = id;
            Name = name;
            IsVisible = isVisible;
        }

        public Category(string name, bool isVisible = true)
        {
            Id = Guid.NewGuid();
            Name = name;
            IsVisible = isVisible;
        }
    }
}
