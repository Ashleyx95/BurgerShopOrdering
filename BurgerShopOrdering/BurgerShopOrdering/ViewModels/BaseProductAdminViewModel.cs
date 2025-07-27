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
    public class BaseProductAdminViewModel : ObservableObject
    {
        protected readonly IMenuService _menuService;
        private Product productToSave;
        private ObservableCollection<Category> categories;
        private ImageSource newImage;
        private bool newImageUploaded;
        private Stream imageStream;
        private bool categoriesVisible;

        public Product ProductToSave
        {
            get { return productToSave; }
            set { SetProperty(ref productToSave, value); }
        }
        public ImageSource NewImage
        {
            get { return newImage; }
            set { SetProperty(ref newImage, value); }
        }
        public bool NewImageUploaded
        {
            get { return newImageUploaded; }
            set { SetProperty(ref newImageUploaded, value); }
        }
        public Stream ImageStream
        {
            get { return imageStream; }
            set { SetProperty(ref imageStream, value); }
        }

        public bool CategoriesVisible
        {
            get { return categoriesVisible; }
            set { SetProperty(ref categoriesVisible, value); }
        }

        public ObservableCollection<Category> Categories
        {
            get { return categories; }
            set { SetProperty(ref categories, value); }
        }

        public BaseProductAdminViewModel(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public ICommand OnCancelCommand => new Command(async () => await Shell.Current.GoToAsync("//MenuAdminPage"));
        public ICommand PickImageCommand => new Command(async () => await PickImageAsync());
        public ICommand ToggleCategoriesVisibilityCommand => new Command(() =>
        {
            CategoriesVisible = !CategoriesVisible;
        });

        protected async Task PickImageAsync()
        {
            var image = await MediaPicker.Default.PickPhotoAsync();
            if (image != null)
            {
                ImageStream = await image.OpenReadAsync();

                NewImage = ImageSource.FromStream(() =>
                {
                    var memoryStream = new MemoryStream();
                    imageStream.CopyTo(memoryStream);
                    memoryStream.Position = 0;
                    return memoryStream;
                });

                NewImageUploaded = true;
            }

        }
        protected void RemoveOldImage()
        {
            if (productToSave.Image != "defaultProduct.jpg")
            {
                var oldImageFilePath = Path.Combine(FileSystem.AppDataDirectory, productToSave.Image);
                if (File.Exists(oldImageFilePath))
                {
                    File.Delete(oldImageFilePath);
                }
            }
        }
        protected async Task SaveNewImage()
        {
            var fileName = _menuService.MakeFileNameSafe(ProductToSave.Name);
            ProductToSave.Image = $"{fileName}.jpg";

            var filePath = Path.Combine(FileSystem.AppDataDirectory, $"{fileName}.jpg");
            using var fileStream = File.Create(filePath);

            imageStream.Position = 0;
            await imageStream.CopyToAsync(fileStream);

            imageStream.Close();
        }
    }
}
