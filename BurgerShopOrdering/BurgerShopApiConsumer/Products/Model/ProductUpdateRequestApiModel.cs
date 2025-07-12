using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopApiConsumer.Products.Model
{
    public class ProductUpdateRequestApiModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ICollection<Guid> CategorieIds { get; set; } = [];
        public string? Image { get; set; }
    }
}
