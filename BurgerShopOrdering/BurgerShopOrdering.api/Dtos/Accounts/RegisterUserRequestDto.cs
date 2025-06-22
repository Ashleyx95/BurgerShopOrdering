using System.ComponentModel.DataAnnotations;

namespace BurgerShopOrdering.api.Dtos.Accounts
{
    public class RegisterUserRequestDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Het wachtwoord en het bevestigingswachtwoord is niet hetzelfde.")]
        public string ConfirmPassword { get; set; }
    }
}
