using BurgerShopOrdering.ViewModels;

namespace BurgerShopOrdering.Pages;

public partial class OrdersPage : ContentPage
{
    private OrdersViewModel viewmodel;
    public OrdersPage(OrdersViewModel viewmodel)
	{
		InitializeComponent();
        BindingContext = this.viewmodel = viewmodel;
    }

    protected override void OnAppearing()
    {
        viewmodel?.OnAppearingCommand.Execute(null);
    }
}