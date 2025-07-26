using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BurgerShopOrdering.ViewModels
{
    public class HomeViewModel
    {
        public ICommand OnMakeOrderClickedCommand => new Command(async () => await Shell.Current.GoToAsync("//MenuPage"));
    }
}
