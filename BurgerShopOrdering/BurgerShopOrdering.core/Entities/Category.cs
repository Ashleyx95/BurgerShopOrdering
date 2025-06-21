using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BurgerShopOrdering.core.Entities
{
    public class Category(Guid id, string name, bool isVisible = true)
    {
        public Guid Id { get; set; } = id;
        public string Name { get; set; } = name;
        public ICollection<Product>? Products { get; set; } = [];
        public bool IsVisible { get; set; } = isVisible;
    }
}
