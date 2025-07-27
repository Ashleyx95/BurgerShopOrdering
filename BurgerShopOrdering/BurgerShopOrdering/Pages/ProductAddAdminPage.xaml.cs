using BurgerShopOrdering.ViewModels;

namespace BurgerShopOrdering.Pages;

public partial class ProductAddAdminPage : ContentPage
{
    private ProductAddAdminViewModel viewmodel;
    public ProductAddAdminPage(ProductAddAdminViewModel viewmodel)
	{
		InitializeComponent();
        BindingContext = this.viewmodel = viewmodel;
    }
    protected override void OnAppearing()
    {
        viewmodel?.OnAppearingCommand.Execute(null);
    }
}