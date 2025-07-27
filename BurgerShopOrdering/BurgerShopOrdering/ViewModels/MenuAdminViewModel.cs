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
    public class MenuAdminViewModel(IMenuService menuService) : BaseMenuViewModel(menuService)
    {
        public ICommand OnAppearingCommand => new Command(async () => await LoadProductsAndCategoriesAsync());
        public ICommand OnCategoryTappedCommand => new Command<Category>(category => OnCategoryTapped(category));

        public ICommand OnCategoryAddTappedCommand => new Command(async () => await Shell.Current.GoToAsync("CategoryAddAdminPage"));
        private async void OnCategoryTapped(Category category)
        {
            foreach (var c in Categories)
            {
                c.IsSelected = false;
            }
            category.IsSelected = true;

            if (category.Name == "Alle producten" || category is null)
            {
                Products = new ObservableCollection<Product>(await _menuService.GetProductsAsync());
                Categories = new ObservableCollection<Category>(Categories);
                UpdateCollectionViewHeight();
            }
            else
            {
                Products = new ObservableCollection<Product>(await _menuService.GetProductsByCategoryAsync(category.Name));
                Categories = new ObservableCollection<Category>(Categories);
                UpdateCollectionViewHeight();
            }
        }

        private async Task LoadProductsAndCategoriesAsync()
        {
            Products = new ObservableCollection<Product>(await _menuService.GetProductsAsync());
            Categories = new ObservableCollection<Category>(await _menuService.GetCategoriesAsync());
            Categories.Insert(0, new Category { Name = "Alle producten", IsSelected = true });
            UpdateCollectionViewHeight();

        }
    }
}
