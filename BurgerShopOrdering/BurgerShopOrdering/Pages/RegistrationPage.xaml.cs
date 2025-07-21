using BurgerShopOrdering.ViewModels;

namespace BurgerShopOrdering.Pages;

public partial class RegistrationPage : ContentPage
{
	public RegistrationPage(RegistrationViewModel viewmodel)
	{
		InitializeComponent();
        BindingContext = viewmodel;
    }
}