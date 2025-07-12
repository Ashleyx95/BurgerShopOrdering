using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopApiConsumer.Products.Model
{
    public class ProductResponseApiModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ICollection<string> Categories { get; set; } = [];
        public string Image { get; set; }
    }
}
