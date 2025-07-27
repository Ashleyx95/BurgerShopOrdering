using BurgerShopOrdering.ViewModels;

namespace BurgerShopOrdering.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel viewmodel)
	{
		InitializeComponent();
        BindingContext = viewmodel;
    }
}