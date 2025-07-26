using BurgerShopOrdering.ViewModels;

namespace BurgerShopOrdering.Pages;

public partial class OrderPage : ContentPage
{
    private OrderViewModel viewmodel;
    public OrderPage(OrderViewModel viewmodel)
	{
		InitializeComponent();
        BindingContext = this.viewmodel = viewmodel;
    }

    protected override void OnAppearing()
    {
        viewmodel?.OnAppearingCommand.Execute(null);
    }
}