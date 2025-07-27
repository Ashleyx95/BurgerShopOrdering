using System.ComponentModel.DataAnnotations;

namespace BurgerShopOrdering.api.Dtos.Accounts
{
    public class LoginUserRequestDto
    {
        [Required(ErrorMessage = "E-mail is verplicht.")]
        [EmailAddress(ErrorMessage = "Voer een geldig e-mailadres in.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Wachtwoord is verplicht.")]
        public string Password { get; set; }
    }
}
