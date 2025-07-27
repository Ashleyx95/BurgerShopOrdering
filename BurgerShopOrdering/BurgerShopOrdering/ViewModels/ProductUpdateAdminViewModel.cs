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
    public class ProductUpdateAdminViewModel(IMenuService menuService) : BaseProductAdminViewModel(menuService)
    {
        public ICommand OnAppearingCommand => new Command(async () => await SetProductAndCategoriesAsync());
        public ICommand OnDisappearingCommand => new Command(() => RemoveProductFromStorage());
        public ICommand OnEditCommand => new Command(async () => await UpdateProductAsync());

        private async Task SetProductAndCategoriesAsync()
        {
            string productJson = await SecureStorage.GetAsync("ProductToSave");

            if (!string.IsNullOrEmpty(productJson))
            {
                ProductToSave = JsonConvert.DeserializeObject<Product>(productJson);
            }

            Categories = new ObservableCollection<Category>(await _menuService.GetCategoriesAsync());

            foreach (var pc in ProductToSave.Categories)
            {
                var c = Categories.First(c => c.Name == pc.Name);
                c.IsSelected = true;
            }
        }
        private void RemoveProductFromStorage()
        {
            SecureStorage.Remove("ProductToEdit");
        }
        private async Task UpdateProductAsync()
        {
            var countSelectedCategories = 0;

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
                RemoveOldImage();
                await SaveNewImage();
            }

            var result = await _menuService.UpdateProductToMenu(ProductToSave);

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
