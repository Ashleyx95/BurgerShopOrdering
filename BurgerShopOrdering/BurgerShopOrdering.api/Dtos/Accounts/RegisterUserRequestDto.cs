using System.ComponentModel.DataAnnotations;

namespace BurgerShopOrdering.api.Dtos.Accounts
{
    public class RegisterUserRequestDto
    {
        [Required(ErrorMessage = "Voornaam is verplicht.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Achternaam is verplicht.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "E-mailadres is verplicht.")]
        [EmailAddress(ErrorMessage = "Voer een geldig e-mailadres in.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Wachtwoord is verplicht.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Bevestig je wachtwoord.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Het wachtwoord en het bevestigingswachtwoord komen niet overeen.")]
        public string ConfirmPassword { get; set; }
    }
}
