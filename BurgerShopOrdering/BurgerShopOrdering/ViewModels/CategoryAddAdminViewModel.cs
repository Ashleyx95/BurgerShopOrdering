using BurgerShopOrdering.Core.Models;
using BurgerShopOrdering.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BurgerShopOrdering.ViewModels
{
    public class CategoryAddAdminViewModel : ObservableObject
    {
        private readonly IMenuService _menuService;
        private Category categoryToAdd;

        public Category CatergoryToAdd
        {
            get { return categoryToAdd; }
            set { SetProperty(ref categoryToAdd, value); }
        }

        public CategoryAddAdminViewModel(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public ICommand OnAppearingCommand => new Command(() => SetCategory());
        public ICommand OnAddCommand => new Command(async () => await SaveCategoryAsync());
        public ICommand OnCancelCommand => new Command(async () => await Shell.Current.GoToAsync("//MenuAdminPage"));

        public void SetCategory()
        {
            categoryToAdd = new Category();
        }

        public async Task SaveCategoryAsync()
        {
            if (string.IsNullOrWhiteSpace(categoryToAdd.Name))
            {
                await App.Current.MainPage.DisplayAlert("Fout", $"Gelieve een naam in te geven", "OK");
                return;
            }

            var result = await _menuService.AddCategory(CatergoryToAdd.Name);

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
