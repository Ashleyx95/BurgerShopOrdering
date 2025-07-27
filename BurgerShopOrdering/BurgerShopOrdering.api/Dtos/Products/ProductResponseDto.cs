namespace BurgerShopOrdering.api.Dtos.Products
{
    public class ProductResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ICollection<string> Categories { get; set; }
        public string Image { get; set; }
    }
}
