using System.ComponentModel.DataAnnotations;

namespace BurgerShopOrdering.api.Dtos.Orders
{
    public class OrderItemCreateRequestDto
    {
        [Required(ErrorMessage = "Product id is verplicht.")]
        public Guid ProductId { get; set; }
        [Required(ErrorMessage = "Aantal is verplicht.")]
        [Range(0, int.MaxValue, ErrorMessage = "Aantal moet 0 of hoger zijn.")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Prijs is verplicht.")]
        [Range(0, double.MaxValue, ErrorMessage = "Prijs moet 0 of hoger zijn.")]
        public decimal Price { get; set; }
    }
}
