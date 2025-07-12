using BurgerShopOrdering.core.Entities;
using System.ComponentModel.DataAnnotations;

namespace BurgerShopOrdering.api.Dtos.Orders
{
    public class OrderCreateRequestDto
    {
        [Required(ErrorMessage = "Minstens één besteld product is vereist.")]
        public ICollection<OrderItemCreateRequestDto> OrderItems { get; set; }
        [Required(ErrorMessage = "Totale prijs is verplicht.")]
        [Range(0, double.MaxValue, ErrorMessage = "Totale prijs moet 0 of hoger zijn.")]
        public decimal TotalPrice { get; set; }
        [Required(ErrorMessage = "Totale hoeveelheid is verplicht.")]
        [Range(0, int.MaxValue, ErrorMessage = "Totale hoeveelheid moet 0 of hoger zijn.")]
        public int TotalQuantity { get; set; }
        [Required(ErrorMessage = "Orderstatus is verplicht.")]
        [EnumDataType(typeof(OrderStatus), ErrorMessage = "Ongeldige orderstatus opgegeven.")]
        public OrderStatus Status { get; set; }
    }
}
