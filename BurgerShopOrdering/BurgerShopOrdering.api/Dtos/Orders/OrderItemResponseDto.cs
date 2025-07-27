namespace BurgerShopOrdering.api.Dtos.Orders
{
    public class OrderItemResponseDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
