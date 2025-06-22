using System.ComponentModel.DataAnnotations;

namespace BurgerShopOrdering.api.Dtos.Accounts
{
    public class LoginUserRequestDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
