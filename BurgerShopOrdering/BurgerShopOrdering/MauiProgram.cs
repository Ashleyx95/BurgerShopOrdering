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

            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IUserApiService, UserApiService>();

            builder.Services.AddTransient<AccountViewModel>();
            builder.Services.AddTransient<AccountPage>();
            builder.Services.AddTransient<HomeAdminPage>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegistrationPage>();
            builder.Services.AddTransient<RegistrationViewModel>();

            return builder.Build();
        }
    }
}
