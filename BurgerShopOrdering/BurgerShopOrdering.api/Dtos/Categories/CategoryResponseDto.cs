using BurgerShopOrdering.api.Dtos.Products;

namespace BurgerShopOrdering.api.Dtos.Categories
{
    public class CategoryResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<ProductResponseDto>? Products { get; set; }
    }
}
