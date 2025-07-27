using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.Core.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ICollection<Category> Categories { get; set; }
        public string Image { get; set; }
        public ImageSource ImageSource { get; set; }
        public int Quantity { get; set; }
    }
}
