using BurgerShopOrdering.Core.Models;
using BurgerShopOrdering.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BurgerShopOrdering.ViewModels
{
    public class OrdersViewModel : ObservableObject
    {
        private readonly IOrderService _orderService;
        private readonly IAccountService _accountService;
        private ObservableCollection<Order> orders;
        private ObservableCollection<OrderFilter> filters;

        public ObservableCollection<Order> Orders
        {
            get { return orders; }
            set { SetProperty(ref orders, value); }
        }

        public ObservableCollection<OrderFilter> Filters
        {
            get { return filters; }
            set { SetProperty(ref filters, value); }
        }

        public OrdersViewModel(IOrderService orderService, IAccountService accountService)
        {
            _orderService = orderService;
            _accountService = accountService;
            Filters = new ObservableCollection<OrderFilter>
            {
                new OrderFilter
                {
                    Name = "Alle bestellingen",
                    IsSelected = true,
                },
                new OrderFilter
                {
                    Name = OrderStatus.Besteld.ToString(),
                    IsSelected = false,
                },
                new OrderFilter
                {
                    Name = OrderStatus.Bereiden.ToString(),
                    IsSelected = false,
                },
                new OrderFilter
                {
                    Name = OrderStatus.Klaar.ToString(),
                    IsSelected = false,
                },
                new OrderFilter
                {
                    Name = OrderStatus.Afgehaald.ToString(),
                    IsSelected = false,
                },
            };
        }

        public ICommand OnAppearingCommand => new Command(async () => await LoadOrdersAsync());
        public ICommand OnFilterOrdersTappedCommand => new Command<OrderFilter>(async (filter) => await OnFilterOrdersTapped(filter));
        public ICommand OnViewOrderTappedCommand => new Command<Order>(async (order) => await ViewOrderAsync(order));

        private async Task ViewOrderAsync(Order order)
        {
            Preferences.Set("orderToShow", JsonConvert.SerializeObject(order));

            var user = await _accountService.GetLoggedInUserAsync();

            if (user?.IsAdmin == true)
            {
                await Shell.Current.GoToAsync("OrderAdminPage");
            }
            else
            {
                await Shell.Current.GoToAsync("OrderPage");
            }
        }

        private async Task OnFilterOrdersTapped(OrderFilter filter)
        {
            foreach (var f in Filters)
            {
                f.IsSelected = false;
            }
            filter.IsSelected = true;

            Filters = new ObservableCollection<OrderFilter>(Filters);

            if (filter.Name == "Alle bestellingen" || filter is null)
            {
                Orders = new ObservableCollection<Order>(await _orderService.GetOrdersAsync());
            }
            else
            {
                Orders = new ObservableCollection<Order>(await _orderService.GetOrdersByStatusAsync(filter.Name));
            }
        }

        private async Task LoadOrdersAsync()
        {
            var ordersFromUser = await _orderService.GetOrdersAsync();
            Orders = new ObservableCollection<Order>(ordersFromUser);
        }
    }
}
