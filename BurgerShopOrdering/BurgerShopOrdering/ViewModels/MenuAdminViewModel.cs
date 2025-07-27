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
        public ICommand OnProductAddTappedCommand => new Command(async () => await Shell.Current.GoToAsync("ProductAddAdminPage"));
        public ICommand OnProductEditTappedCommand => new Command<Product>(async (product) =>
        {
            string productJson = JsonConvert.SerializeObject(product);

            await SecureStorage.SetAsync("ProductToSave", productJson);

            await Shell.Current.GoToAsync("ProductUpdateAdminPage");
        });
        public ICommand OnCategoryAddTappedCommand => new Command(async () => await Shell.Current.GoToAsync("CategoryAddAdminPage"));
        public ICommand OnCategoryDeleteTappedCommand => new Command(async () => await DeleteCategoryAsync());

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

        public async Task DeleteCategoryAsync()
        {
            var category = Categories.First(c => c.IsSelected);

            if (category.Name == "Alle producten")
            {
                await App.Current.MainPage.DisplayAlert("Fout", $"Deze categorie kan niet verwijderd worden", "OK");
                return;
            }

            bool isConfirmed = await App.Current.MainPage.DisplayAlert(
                "Bevestiging",
                $"Weet je zeker dat je de categorie '{category.Name}' wilt verwijderen?",
                "Ja", "Nee");

            if (!isConfirmed)
                return;

            var result = await _menuService.RemoveCategory(category);

            if (!result.Success)
            {
                var errorMessage = result.Message;

                if (result.Errors.Any())
                    errorMessage += Environment.NewLine + string.Join(Environment.NewLine, result.Errors);

                await App.Current.MainPage.DisplayAlert("Fout", errorMessage, "OK");
                return;
            }

            await LoadProductsAndCategoriesAsync();
        }
        }
    }
}
