namespace BurgerShopOrdering.api.Dtos.Orders
{
    public class OrderResponseDto
    {
        public Guid Id { get; set; }
        public string NameUser { get; set; }
        public string ApplicationUserId { get; set; }
        public string Name { get; set; }
        public ICollection<OrderItemResponseDto> OrderItems { get; set; }
        public decimal TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
        public string Status { get; set; }
        public DateTime DateOrdered { get; set; }
        public DateTime? DateDelivered { get; set; }
    }
}
