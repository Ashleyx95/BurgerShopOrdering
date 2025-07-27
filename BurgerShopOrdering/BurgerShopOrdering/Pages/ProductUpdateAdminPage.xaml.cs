using BurgerShopOrdering.ViewModels;

namespace BurgerShopOrdering.Pages;

public partial class ProductUpdateAdminPage : ContentPage
{
    private ProductUpdateAdminViewModel viewmodel;
    public ProductUpdateAdminPage(ProductUpdateAdminViewModel viewmodel)
	{
		InitializeComponent();
        BindingContext = this.viewmodel = viewmodel;
    }
    protected override void OnAppearing()
    {
        viewmodel?.OnAppearingCommand.Execute(null);
    }
    protected override void OnDisappearing()
    {
        viewmodel?.OnDisappearingCommand.Execute(null);
    }
}