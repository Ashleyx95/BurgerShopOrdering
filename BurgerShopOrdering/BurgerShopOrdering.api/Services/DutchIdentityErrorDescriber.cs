using Microsoft.AspNetCore.Identity;

namespace BurgerShopOrdering.api.Services
{
    public class DutchIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DefaultError()
            => new IdentityError
            {
                Code = nameof(DefaultError),
                Description = "Er is een onbekende fout opgetreden."
            };

        public override IdentityError DuplicateEmail(string email)
            => new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = $"Het e-mailadres '{email}' is al in gebruik."
            };

        public override IdentityError DuplicateUserName(string userName)
            => new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = $"De gebruikersnaam '{userName}' is al in gebruik."
            };

        public override IdentityError InvalidEmail(string? email)
            => new IdentityError
            {
                Code = nameof(InvalidEmail),
                Description = $"'{email ?? "Dit"}' is geen geldig e-mailadres."
            };

        public override IdentityError InvalidUserName(string? userName)
            => new IdentityError
            {
                Code = nameof(InvalidUserName),
                Description = $"De gebruikersnaam '{userName}' is ongeldig."
            };

        public override IdentityError PasswordMismatch()
            => new IdentityError
            {
                Code = nameof(PasswordMismatch),
                Description = "Het opgegeven wachtwoord is onjuist."
            };

        public override IdentityError PasswordRequiresDigit() 
            => new IdentityError
            {
                Code = nameof(PasswordRequiresDigit),
                Description = "Het wachtwoord moet minstens één cijfer bevatten."
            };

        public override IdentityError PasswordRequiresLower()
            => new IdentityError
            {
                Code = nameof(PasswordRequiresLower),
                Description = "Het wachtwoord moet minstens één kleine letter bevatten."
            };

        public override IdentityError PasswordRequiresNonAlphanumeric()
            => new IdentityError
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Description = "Het wachtwoord moet minstens één speciaal teken bevatten."
            };

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
            => new IdentityError
            {
                Code = nameof(PasswordRequiresUniqueChars),
                Description = $"Het wachtwoord moet minstens {uniqueChars} unieke tekens bevatten."
            };

        public override IdentityError PasswordRequiresUpper()
            => new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = "Het wachtwoord moet minstens één hoofdletter bevatten."
            };

        public override IdentityError PasswordTooShort(int length) 
            => new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = $"Het wachtwoord moet minstens {length} tekens bevatten."
            };

    }
}
