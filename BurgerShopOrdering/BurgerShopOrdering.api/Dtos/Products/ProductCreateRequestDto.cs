using System.ComponentModel.DataAnnotations;

namespace BurgerShopOrdering.api.Dtos.Products
{
    public class ProductCreateRequestDto
    {
        [Required(ErrorMessage = "Naam is verplicht.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Prijs is verplicht.")]
        [Range(0, double.MaxValue, ErrorMessage = "Prijs mag niet lager zijn dan 0.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Minstens één categorie is verplicht.")]
        public ICollection<Guid> CategoryIds { get; set; }
        public string? Image { get; set; }
    }
}
