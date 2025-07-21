using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.Core.Services.Interfaces
{
    public interface INavigationService
    {
        void UpdateTabBarForRole(string role);
    }
}
