using BurgerShopOrdering.Core.Models;
using BurgerShopOrdering.Core.Services.Interfaces;
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
    public class OrderAdminViewModel : BaseOrderViewModel
    {
        private ObservableCollection<string> orderStatuses;
        private string selectedStatus;

        public string SelectedStatus
        {
            get { return selectedStatus; }
            set { SetProperty(ref selectedStatus, value); }
        }
        public ObservableCollection<string> OrderStatuses
        {
            get { return orderStatuses; }
            set { SetProperty(ref orderStatuses, value); }

        }
        public OrderAdminViewModel(IOrderService orderService) : base(orderService)
        {
            orderStatuses = new ObservableCollection<string>
            {
                OrderStatus.Bereiden.ToString(),
                OrderStatus.Klaar.ToString(),
                OrderStatus.Afgehaald.ToString(),
            };
        }

        public ICommand OnOrderStatusChangedCommand => new Command(async () => await ChangeOrderStatusAsync());

        private async Task ChangeOrderStatusAsync()
        {
            if (Order.Status.ToString() == selectedStatus)
            {
                return;
            }

            Order.Status = (OrderStatus)Enum.Parse(typeof(OrderStatus), selectedStatus);

            var result = await _orderService.UpdateOrderStatusAsync(Order);

            if (result.Success)
            {
                Order = new Order
                {
                    DateDelivered = Order.DateDelivered,
                    TotalPrice = Order.TotalPrice,
                    OrderItems = Order.OrderItems,
                    DateOrdered = Order.DateOrdered,
                    Id = Order.Id,
                    Name = Order.Name,
                    NameUser = Order.NameUser,
                    Status = Order.Status,
                    TotalQuantity = Order.TotalQuantity,
                };
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
