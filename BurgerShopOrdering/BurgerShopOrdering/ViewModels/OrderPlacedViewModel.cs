using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BurgerShopOrdering.ViewModels
{
    public class OrderPlacedViewModel
    {
        public ICommand OnShowOverviewOrdersCommand => new Command(async () =>
        {
            await Shell.Current.GoToAsync("..");
            await Shell.Current.GoToAsync("//AccountPage");
            await Shell.Current.GoToAsync("OrdersPage");
        });
    }
}
