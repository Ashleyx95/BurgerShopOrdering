using BurgerShopOrdering.Core.Models;
using BurgerShopOrdering.Core.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BurgerShopOrdering.ViewModels
{
    public class OrderViewModel(IOrderService orderService) : BaseOrderViewModel(orderService)
    {
        public ICommand OnAppearingCommand => new Command( () => SetOrderToShow());

        private void SetOrderToShow()
        {
            var orderJson = Preferences.Get("orderToShow", string.Empty);
            var orderToShow = JsonConvert.DeserializeObject<Order>(orderJson);

            Order = orderToShow == null ? new Order { OrderItems = [] } : orderToShow;
        }
    }
}
