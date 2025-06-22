using System.ComponentModel.DataAnnotations;

namespace BurgerShopOrdering.api.Dtos.Categories
{
    public class CategoryCreateRequestDto
    {
        [Required(ErrorMessage = "Naam is verplicht.")]
        public string Name { get; set; }
    }
}
