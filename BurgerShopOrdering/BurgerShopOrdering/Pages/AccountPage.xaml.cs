using BurgerShopOrdering.ViewModels;

namespace BurgerShopOrdering.Pages;

public partial class AccountPage : ContentPage
{
    private AccountViewModel viewmodel;
    public AccountPage(AccountViewModel viewmodel)
	{
		InitializeComponent();
        BindingContext = this.viewmodel = viewmodel;
    }

    protected override void OnAppearing()
    {
        viewmodel?.OnAppearingCommand.Execute(null);
    }
}