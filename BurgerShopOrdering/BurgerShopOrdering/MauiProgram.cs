using BurgerShopApiConsumer.Categories;
using BurgerShopApiConsumer.Orders;
using BurgerShopApiConsumer.Products;
using BurgerShopApiConsumer.Users;
using BurgerShopOrdering.Core.Services.Interfaces;
using BurgerShopOrdering.Core.Services.Web;
using BurgerShopOrdering.Pages;
using BurgerShopOrdering.ViewModels;
using Microsoft.Extensions.Logging;

namespace BurgerShopOrdering
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<IAccountService, AccountService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<IMenuService, MenuService>();
            builder.Services.AddSingleton<IOrderService, OrderService>();

            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IUserApiService, UserApiService>();
            builder.Services.AddScoped<IProductApiService, ProductApiService>();
            builder.Services.AddScoped<ICategoryApiService, CategoryApiService>();
            builder.Services.AddScoped<IOrderApiService, OrderApiService>();

            builder.Services.AddTransient<AccountViewModel>();
            builder.Services.AddTransient<AccountPage>();
            builder.Services.AddTransient<HomeAdminPage>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegistrationPage>();
            builder.Services.AddTransient<RegistrationViewModel>();
            builder.Services.AddTransient<MenuPage>();
            builder.Services.AddTransient<MenuAdminPage>();
            builder.Services.AddTransient<BaseMenuViewModel>();
            builder.Services.AddTransient<MenuViewModel>();
            builder.Services.AddTransient<MenuAdminViewModel>();
            builder.Services.AddTransient<ShoppingCartPage>();
            builder.Services.AddTransient<ShoppingCartViewModel>();
            builder.Services.AddTransient<OrdersPage>();
            builder.Services.AddTransient<OrdersViewModel>();
            builder.Services.AddTransient<OrderPage>();
            builder.Services.AddTransient<OrderViewModel>();
            builder.Services.AddTransient<BaseOrderViewModel>();
            builder.Services.AddTransient<OrderPlacedPage>();
            builder.Services.AddTransient<OrderPlacedViewModel>();

            return builder.Build();
        }
    }
}
