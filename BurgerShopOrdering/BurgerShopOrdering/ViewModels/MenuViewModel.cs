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
    public class MenuViewModel : BaseMenuViewModel
    {
        private readonly IOrderService _orderService;

        private Order order;
        public Order Order
        {
            get { return order; }
            set { SetProperty(ref order, value); }

        }
        public MenuViewModel(IMenuService menuService, IOrderService orderService) : base(menuService)
        {
            _orderService = orderService;
        }

        public ICommand OnAppearingCommand => new Command(async () => await LoadProductsAndCategoriesAsync());
        public ICommand IncrementQuantityCommand => new Command<Product>(product => IncrementQuantity(product));
        public ICommand DecrementQuantityCommand => new Command<Product>(product => DecrementQuantity(product));
        public ICommand OnCategoryTappedCommand => new Command<Category>(category => FilterProductsByCategory(category));
        public ICommand ManualChangedQuantityCommand => new Command<Product>(product => ManualChangedQuantity(product));

        private async Task LoadProductsAndCategoriesAsync()
        {
            Products = new ObservableCollection<Product>(await _menuService.GetProductsAsync());
            Categories = new ObservableCollection<Category>(await _menuService.GetCategoriesAsync());
            Categories.Insert(0, new Category { Name = "Alle producten", IsSelected = true });
            Order = _orderService.GetOrderFromStorage();

            UpdateCollectionViewHeight();
            SyncProductQuantitiesWithOrder();
        }

        private async void FilterProductsByCategory(Category category)
        {
            foreach (var c in Categories)
            {
                c.IsSelected = false;
            }

            category.IsSelected = true;

            if (category.Name == "Alle producten" || category is null)
            {
                Products = new ObservableCollection<Product>(await _menuService.GetProductsAsync());
            }
            else
            {
                Products = new ObservableCollection<Product>(await _menuService.GetProductsByCategoryAsync(category.Name));
            }

            Categories = new ObservableCollection<Category>(Categories);
            SyncProductQuantitiesWithOrder();
            UpdateCollectionViewHeight();
        }
        private void DecrementQuantity(Product product)
        {
            product.Quantity = product.Quantity > 0? product.Quantity - 1 : 0;
            
            UpdateOrder(product);
        }

        private void IncrementQuantity(Product product)
        {
            product.Quantity = product.Quantity > 19 ? 20 : product.Quantity + 1;

            UpdateOrder(product);
        }

        private void ManualChangedQuantity(Product product)
        {
            product.Quantity = product.Quantity > 0 ? product.Quantity : 0;
            product.Quantity = product.Quantity > 19 ? 20 : product.Quantity;
            UpdateOrder(product);
        }

        private void SyncProductQuantitiesWithOrder()
        {
            if (Order.OrderItems.Count == 0)
            {
                foreach (var p in Products)
                {
                    p.Quantity = 0;
                }
            }
            else
            {
                foreach (var p in Products)
                {
                    var orderItem = Order.OrderItems.FirstOrDefault(oi => oi.ProductId == p.Id);

                    p.Quantity = orderItem?.Quantity ?? 0;
                }
            }
        }

        void UpdateOrder(Product product)
        {
            var orderItem = Order.OrderItems.FirstOrDefault(o => o.ProductId == product.Id);

            if (orderItem == null && product.Quantity > 0)
            {
                orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    TotalPrice = _orderService.CalculateTotalPriceOrderItem(product.Price, product.Quantity),
                    Quantity = product.Quantity,
                    ImageSource = product.ImageSource,
                };
                Order.OrderItems.Add(orderItem);
            }
            else if (orderItem != null)
            {
                if (product.Quantity == 0)
                {
                    Order.OrderItems.Remove(orderItem);
                }
                else
                {
                    orderItem.Quantity = product.Quantity;
                }
            }

            _orderService.StoreOrderInStorage(Order);
            Products = new ObservableCollection<Product>(Products);
        }
    }
}
