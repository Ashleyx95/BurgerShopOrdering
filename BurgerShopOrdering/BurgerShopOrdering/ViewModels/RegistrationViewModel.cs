using BurgerShopOrdering.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BurgerShopOrdering.ViewModels
{
    public class RegistrationViewModel : ObservableObject
    {
        private readonly IAccountService accountService;

        private string email;
        private string password;
        private string confirmPassword;
        private string firstname;
        private string lastname;

        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }

        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }
        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set { SetProperty(ref confirmPassword, value); }
        }
        public string FirstName
        {
            get { return firstname; }
            set { SetProperty(ref firstname, value); }
        }
        public string LastName
        {
            get { return lastname; }
            set { SetProperty(ref lastname, value); }
        }

        public RegistrationViewModel(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        public ICommand OnRegistrationClickedCommand => new Command(async () => await TryRegisterAsync());

        private async Task TryRegisterAsync()
        {
            if (!ValidateInputs(out string validationError))
            {
                await App.Current.MainPage.DisplayAlert("Fout", validationError, "OK");
                return;
            }

            var result = await accountService.TryRegisterAsync(FirstName.Trim(), LastName.Trim(), Email.Trim(), Password, ConfirmPassword);

            if (result.Success)
            {
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                var errorMessage = result.Message;

                if (result.Errors.Any())
                    errorMessage += Environment.NewLine + string.Join(Environment.NewLine, result.Errors);

                await App.Current.MainPage.DisplayAlert("Fout", errorMessage, "OK");
            }
        }

        private bool ValidateInputs(out string error)
        {
            if (string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(ConfirmPassword) ||
                string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName))
            {
                error = "Gelieve alle velden in te geven";
                return false;
            }

            if (!new EmailAddressAttribute().IsValid(Email))
            {
                error = "Gelieve een geldig e-mailadres in te geven";
                return false;
            }

            if (Password != ConfirmPassword)
            {
                error = "Wachtwoord en bevestigingswachtwoord moeten hetzelfde zijn";
                return false;
            }

            error = string.Empty;
            return true;
        }
    }
}
