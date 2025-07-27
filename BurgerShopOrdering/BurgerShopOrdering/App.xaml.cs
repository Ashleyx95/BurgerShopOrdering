using System.Globalization;

namespace BurgerShopOrdering
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            CultureInfo.CurrentCulture = new CultureInfo("nl-BE");
            CultureInfo.CurrentUICulture = new CultureInfo("nl-BE");

            MainPage = new AppShell();
        }
    }
}
