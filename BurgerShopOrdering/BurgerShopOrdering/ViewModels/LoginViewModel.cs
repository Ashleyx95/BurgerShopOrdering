using BurgerShopOrdering.Core.Services.Interfaces;
using BurgerShopOrdering.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BurgerShopOrdering.ViewModels
{
    public class LoginViewModel : ObservableObject
    {
        private readonly IAccountService _accountService;

        private string email;
        private string password;

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
        public LoginViewModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public ICommand OnLoginClickedCommand => new Command(async () => await TryLoginAsync());
        public ICommand OnRegistrationLinkClickedCommand => new Command(async () => await Shell.Current.GoToAsync("RegistrationPage"));

        private async Task TryLoginAsync()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Fout", "Gelieve een email en wachtwoord in te geven", "OK");
                return;
            }

            var result = await _accountService.TryLoginAsync(Email, Password);

            if (result.Success)
            {
                Password = "";
                var user = await _accountService.GetLoggedInUserAsync();

                if (user == null)
                {
                    await App.Current.MainPage.DisplayAlert("Fout", "Gebruiker kon niet worden geladen na het inloggen.", "OK");
                    return;
                }

                if (user.IsAdmin)
                {
                    await Shell.Current.GoToAsync("..");
                    await Task.Delay(1);
                    await Shell.Current.GoToAsync("//HomeAdminPage");
                }
                else
                {
                    string lastPage = Preferences.Get("LastPage", "HomePage");

                    if (lastPage == "HomePage")
                    {
                        await Shell.Current.GoToAsync("..");
                        await Shell.Current.GoToAsync("//HomePage");
                    }
                    else
                    {
                        await Shell.Current.GoToAsync("..");
                        await Shell.Current.GoToAsync(lastPage);
                    }
                    Preferences.Remove("LastPage");
                }
            }
            else
            {
                var errorMessage = result.Message;

                if (result.Errors.Any())
                    errorMessage += Environment.NewLine + string.Join(Environment.NewLine, result.Errors);

                await App.Current.MainPage.DisplayAlert("Fout", errorMessage, "OK");
            }
        }
    }
}
