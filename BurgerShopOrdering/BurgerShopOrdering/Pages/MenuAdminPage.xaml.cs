using BurgerShopOrdering.ViewModels;

namespace BurgerShopOrdering.Pages;

public partial class MenuAdminPage : ContentPage
{
    private MenuAdminViewModel viewmodel;
    public MenuAdminPage(MenuAdminViewModel viewmodel)
	{
		InitializeComponent();
        BindingContext = this.viewmodel = viewmodel;
    }

    protected override void OnAppearing()
    {
        viewmodel?.OnAppearingCommand.Execute(null);
    }
}