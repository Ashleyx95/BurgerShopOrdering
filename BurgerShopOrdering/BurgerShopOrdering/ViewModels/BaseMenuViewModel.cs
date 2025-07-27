using BurgerShopOrdering.Core.Models;
using BurgerShopOrdering.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.ViewModels
{
    public class BaseMenuViewModel : ObservableObject
    {
        protected readonly IMenuService _menuService;

        private ObservableCollection<Product> products = [];
        private ObservableCollection<Category> categories = [];
        private double collectionViewHeight;

        public double CollectionViewHeight
        {
            get { return collectionViewHeight; }
            set { SetProperty(ref collectionViewHeight, value); }
        }

        public ObservableCollection<Product> Products
        {
            get { return products; }
            set { SetProperty(ref products, value); }
        }

        public ObservableCollection<Category> Categories
        {
            get { return categories; }
            set { SetProperty(ref categories, value); }
        }

        public BaseMenuViewModel(IMenuService menuService)
        {
            _menuService = menuService;
        }

        //Method for the bug in android to show the Collection.EmptyViewTemplate
        protected void UpdateCollectionViewHeight()
        {
            if (Products == null || Products.Count == 0)
            {
                var displayInfo = DeviceDisplay.MainDisplayInfo;
                var screenHeight = displayInfo.Height / displayInfo.Density;

                CollectionViewHeight = screenHeight * 0.6;
            }
            else
            {
                CollectionViewHeight = -1;
            }
        }
    }
}
