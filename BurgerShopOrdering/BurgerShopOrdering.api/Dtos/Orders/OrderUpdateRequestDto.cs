using BurgerShopOrdering.core.Entities;
using System.ComponentModel.DataAnnotations;

namespace BurgerShopOrdering.api.Dtos.Orders
{
    public class OrderUpdateRequestDto
    {
        [Required(ErrorMessage = "Id is verplicht.")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Orderstatus is verplicht.")]
        [EnumDataType(typeof(OrderStatus), ErrorMessage = "Ongeldige orderstatus opgegeven.")]
        public OrderStatus Status { get; set; }
    }
}
