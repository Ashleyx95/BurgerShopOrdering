using BurgerShopOrdering.Core.Models;
using BurgerShopOrdering.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BurgerShopOrdering.ViewModels
{
    public class ShoppingCartViewModel : ObservableObject
    {
        private readonly IOrderService _orderService;
        private readonly IAccountService _accountService;

        private Order order;
        public Order Order
        {
            get { return order; }
            set { SetProperty(ref order, value); }

        }
        public ShoppingCartViewModel(IOrderService orderService, IAccountService accountService)
        {
            _orderService = orderService;
            _accountService = accountService;
        }
        public ICommand OnAppearingCommand => new Command(() => SetOrder());
        public ICommand OnPlaceOrderClickedCommand => new Command(async () => await PlaceOrderAsync());
        public ICommand QuantityChangedCommando => new Command<OrderItem>(orderItem => ChangeQuantityOrderItem(orderItem));
        public ICommand OnDeleteOrderItemCommand => new Command<OrderItem>(orderItem => DeleteOrderItem(orderItem));

        private void DeleteOrderItem(OrderItem orderItem)
        {
            Order.OrderItems.Remove(orderItem);
            Order.TotalPrice = _orderService.CalculateTotalPriceOrder(Order);
            Order.TotalQuantity = _orderService.CalculateTotalItemsInOrder(Order);

            Order = new Order()
            {
                OrderItems = new ObservableCollection<OrderItem>(Order.OrderItems),
                TotalPrice = Order.TotalPrice,
                TotalQuantity = Order.TotalQuantity,
            };

            _orderService.StoreOrderInStorage(Order);
        }

        private void ChangeQuantityOrderItem(OrderItem orderItem)
        {
            orderItem.Quantity = orderItem.Quantity > 0 ? orderItem.Quantity : 0;
            orderItem.Quantity = orderItem.Quantity > 19 ? 20 : orderItem.Quantity;

            if (orderItem.Quantity == 0)
            {
                Order.OrderItems.Remove(orderItem);
            }
            else
            {
                orderItem.TotalPrice = _orderService.CalculateTotalPriceOrderItem(orderItem.ProductPrice, orderItem.Quantity);
            }

            Order.TotalPrice = _orderService.CalculateTotalPriceOrder(Order);
            Order.TotalQuantity = _orderService.CalculateTotalItemsInOrder(Order);

            Order = new Order()
            {
                OrderItems = new ObservableCollection<OrderItem>(Order.OrderItems),
                TotalPrice = Order.TotalPrice,
                TotalQuantity = Order.TotalQuantity,
            };

            _orderService.StoreOrderInStorage(Order);
        }

        private async Task PlaceOrderAsync()
        {
            var userLoggedIn = await _accountService.IsAuthenticatedAsync();

            if (!userLoggedIn)
            {
                await Shell.Current.GoToAsync("//AccountPage");
                await Shell.Current.GoToAsync("LoginPage");
                Preferences.Set("LastPage", "ShoppingCartPage");
                return;
            }

            bool answer = await Application.Current.MainPage.DisplayAlert(
                        "Bevestiging",
                        "Ben je zeker dat je dit wilt bestellen?",
                        "Ja",
                        "Nee");

            if (answer)
            {
                var result = await _orderService.CreateOrderAsync(Order);

                if (result.Success)
                {
                    _orderService.RemoveOrderFromStorage(Order);
                    await Shell.Current.GoToAsync("OrderPlacedPage");
                    return;
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

        private void SetOrder()
        {
            Order = _orderService.GetOrderFromStorage();
            Order.TotalQuantity = _orderService.CalculateTotalItemsInOrder(Order);
            Order.TotalPrice = _orderService.CalculateTotalPriceOrder(Order);
            Order = new Order
            {
                OrderItems = Order.OrderItems,
                TotalPrice = Order.TotalPrice,
                TotalQuantity = Order.TotalQuantity,
            };
            _orderService.StoreOrderInStorage(Order);
        }
    }
}
