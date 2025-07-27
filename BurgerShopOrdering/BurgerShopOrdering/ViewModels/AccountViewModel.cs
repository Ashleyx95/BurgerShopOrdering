using BurgerShopOrdering.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BurgerShopOrdering.ViewModels
{
    public class AccountViewModel : ObservableObject
    {
        private readonly IAccountService _accountService;

        private bool isLoggedIn;
        private bool isLoggedOut;
        private string fullName = "";
        private string userName = "";
        private bool isAdmin;
        private bool isClient;

        public string UserName
        {
            get { return userName; }
            set { SetProperty(ref userName, value); }
        }
        public string FullName
        {
            get { return fullName; }
            set { SetProperty(ref fullName, value); }
        }
        public bool IsLoggedIn
        {
            get { return isLoggedIn; }
            set { SetProperty(ref isLoggedIn, value); }
        }
        public bool IsLoggedOut
        {
            get { return isLoggedOut; }
            set { SetProperty(ref isLoggedOut, value); }
        }
        public bool IsAdmin
        {
            get { return isAdmin; }
            set { SetProperty(ref isAdmin, value); }
        }
        public bool IsClient
        {
            get { return isClient; }
            set { SetProperty(ref isClient, value); }
        }

        public AccountViewModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public ICommand OnAppearingCommand => new Command(async () => await CheckIfUserIsLoggedIn());
        public ICommand OnLoginClickedCommand => new Command(async () => await Shell.Current.GoToAsync("LoginPage"));
        public ICommand OnLogoutClickedCommand => new Command(async () => await TryLogoutAsync());
        public ICommand OnOrderClientClickedCommand => new Command(async () => await Shell.Current.GoToAsync("OrdersPage"));
        public ICommand OnOrderAdminClickedCommand => new Command(async () => await Shell.Current.GoToAsync("//OrdersPage"));
        public ICommand OnMenuAmendClickedCommand => new Command(async () => await Shell.Current.GoToAsync("//MenuAdminPage"));

        private async Task TryLogoutAsync()
        {
            _accountService.Logout();
            await CheckIfUserIsLoggedIn();
            await Shell.Current.GoToAsync("//AccountPage");
        }

        private async Task CheckIfUserIsLoggedIn()
        {
            IsLoggedIn = await _accountService.IsAuthenticatedAsync();
            IsLoggedOut = !IsLoggedIn;

            var user = await _accountService.GetLoggedInUserAsync();

            if (user != null)
            {
                FullName = $"{user.FirstName} {user.LastName}";
                UserName = user.Email;
                IsAdmin = user.IsAdmin;
                IsClient = !user.IsAdmin;
            }
            else
            {
                FullName = "";
                UserName = "";
                IsAdmin = false;
                IsClient = false;
            }
        }
    }
}
