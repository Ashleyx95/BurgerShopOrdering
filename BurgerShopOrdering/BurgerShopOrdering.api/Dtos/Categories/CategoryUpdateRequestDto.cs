using System.ComponentModel.DataAnnotations;

namespace BurgerShopOrdering.api.Dtos.Categories
{
    public class CategoryUpdateRequestDto
    {
        [Required(ErrorMessage = "Id is verplicht.")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Naam is verplicht.")]
        public string Name { get; set; }
    }
}
