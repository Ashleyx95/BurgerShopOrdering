using BurgerShopOrdering.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.Core.Services.Web
{
    public class NavigationService : INavigationService
    {
        public void UpdateTabBarForRole(string role)
        {
            var appShell = (AppShell)Shell.Current;
            appShell.UpdateTabBarForRole(role);
        }
    }
}
