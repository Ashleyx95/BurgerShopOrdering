﻿using BurgerShopOrdering.Core.Models;
using BurgerShopOrdering.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BurgerShopOrdering.ViewModels
{
    public class BaseOrderViewModel : ObservableObject
    {
        protected readonly IOrderService _orderService;
        protected Order order;
        public Order Order
        {
            get { return order; }
            set { SetProperty(ref order, value); }
        }
        public BaseOrderViewModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public ICommand OnAppearingCommand => new Command(() => SetOrderToShow());

        private void SetOrderToShow()
        {
            var orderJson = Preferences.Get("orderToShow", string.Empty);
            var orderToShow = JsonConvert.DeserializeObject<Order>(orderJson);

            Order = orderToShow == null ? new Order { OrderItems = [] } : orderToShow;
        }
    }
}
