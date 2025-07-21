using BurgerShopOrdering.Core.Models;
using BurgerShopOrdering.Pages;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Newtonsoft.Json;

namespace BurgerShopOrdering
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            InitializeAsync();
            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
        }

        private async Task InitializeAsync()
        {
            string role = await GetUserRoleAsync();
            UpdateTabBarForRole(role);
        }

        private async Task<string> GetUserRoleAsync()
        {
            string? userJson = await SecureStorage.GetAsync("CurrentUser");

            if (string.IsNullOrEmpty(userJson))
            {
                return "";
            }

            var user = JsonConvert.DeserializeObject<User>(userJson);

            if (user.IsAdmin)
            {
                return "Admin";
            }
            return "";
        }

        public void UpdateTabBarForRole(string role)
        {
            if (role == "Admin")
            {
                ClientTabBar.IsVisible = false;
                AdminTabBar.IsVisible = true;
            }
            else
            {
                AdminTabBar.IsVisible = false;
                ClientTabBar.IsVisible = true;
            }
        }
    }
}
