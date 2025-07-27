using BurgerShopOrdering.ViewModels;

namespace BurgerShopOrdering.Pages;

public partial class HomePage : ContentPage
{
	public HomePage(HomeViewModel viewmodel)
	{
		InitializeComponent();
        BindingContext = viewmodel;
    }
}