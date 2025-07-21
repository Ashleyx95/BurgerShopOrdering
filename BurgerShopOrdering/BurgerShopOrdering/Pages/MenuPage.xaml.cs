using BurgerShopOrdering.Core.Models;
using BurgerShopOrdering.ViewModels;

namespace BurgerShopOrdering.Pages;

public partial class MenuPage : ContentPage
{
    private MenuViewModel viewmodel;
    public MenuPage(MenuViewModel viewmodel)
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
        var product = (Product)entry.BindingContext;
        viewmodel.ManualChangedQuantityCommand.Execute(product);
    }
}