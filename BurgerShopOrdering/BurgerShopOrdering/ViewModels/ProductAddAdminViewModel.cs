using BurgerShopOrdering.Core.Models;
using BurgerShopOrdering.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BurgerShopOrdering.ViewModels
{
    public class ProductAddAdminViewModel(IMenuService menuService) : BaseProductAdminViewModel(menuService)
    {
        public ICommand OnAppearingCommand => new Command(async () => await SetProductAndCategoriesAsync());
        public ICommand OnAddCommand => new Command(async () => await SaveProductAsync());

        private async Task SetProductAndCategoriesAsync()
        {
            Categories = new ObservableCollection<Category>(await _menuService.GetCategoriesAsync());

            ProductToSave = new Product
            {
                Categories = new ObservableCollection<Category>()
            };
        }
        private async Task SaveProductAsync()
        {
            if (string.IsNullOrWhiteSpace(ProductToSave.Name) || ProductToSave.Price == 0)
            {
                await App.Current.MainPage.DisplayAlert("Fout", $"Gelieve alle velden in te geven", "OK");
                return;
            }

            if (ProductToSave.Price <= 0)
            {
                await App.Current.MainPage.DisplayAlert("Fout", $"Gelieve een prijs boven €0 te kiezen", "OK");
                return;
            }

            var selectedCategories = Categories.Where(c => c.IsSelected).ToList();

            if (!selectedCategories.Any())
            {
                await App.Current.MainPage.DisplayAlert("Fout", $"Gelieve minstens één categorie te selecteren", "OK");
                return;
            }

            ProductToSave.Categories = selectedCategories;

            if (ImageStream != null)
            {
                await SaveNewImage();
            }

            var result = await _menuService.AddNewProductToMenu(ProductToSave);

            if (result.Success)
            {
                await Shell.Current.GoToAsync("//MenuAdminPage");
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
