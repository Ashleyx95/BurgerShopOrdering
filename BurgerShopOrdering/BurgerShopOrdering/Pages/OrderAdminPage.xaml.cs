using BurgerShopOrdering.ViewModels;

namespace BurgerShopOrdering.Pages;

public partial class OrderAdminPage : ContentPage
{
    private OrderAdminViewModel viewmodel;
    public OrderAdminPage(OrderAdminViewModel viewmodel)
	{
		InitializeComponent();
        BindingContext = this.viewmodel = viewmodel;
    }
    protected override void OnAppearing()
    {
        viewmodel?.OnAppearingCommand.Execute(null);
    }
}