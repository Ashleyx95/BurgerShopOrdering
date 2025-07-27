using BurgerShopOrdering.Core.Models;
using BurgerShopOrdering.ViewModels;

namespace BurgerShopOrdering.Pages;

public partial class ShoppingCartPage : ContentPage
{
    private ShoppingCartViewModel viewmodel;
    public ShoppingCartPage(ShoppingCartViewModel viewmodel)
	{
		InitializeComponent();
        BindingContext = this.viewmodel = viewmodel;
    }
    protected override void OnAppearing()
    {
        viewmodel?.OnAppearingCommand.Execute(null);
    }
    private void OnEntryCompleted(object sender, EventArgs e)
    {
        var entry = (Entry)sender;
        var orderItem = (OrderItem)entry.BindingContext;
        viewmodel.QuantityChangedCommando.Execute(orderItem);
    }

}