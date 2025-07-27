using BurgerShopOrdering.ViewModels;

namespace BurgerShopOrdering.Pages;

public partial class OrderPlacedPage : ContentPage
{
	public OrderPlacedPage(OrderPlacedViewModel viewmodel)
	{
		InitializeComponent();
        BindingContext = viewmodel;
    }
}