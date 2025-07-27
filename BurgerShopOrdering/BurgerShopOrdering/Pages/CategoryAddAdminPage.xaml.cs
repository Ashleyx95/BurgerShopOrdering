using BurgerShopOrdering.ViewModels;

namespace BurgerShopOrdering.Pages;

public partial class CategoryAddAdminPage : ContentPage
{
    private readonly CategoryAddAdminViewModel viewmodel;
    public CategoryAddAdminPage(CategoryAddAdminViewModel viewmodel)
	{
		InitializeComponent();
        BindingContext = this.viewmodel = viewmodel;
    }
    protected override void OnAppearing()
    {
        viewmodel?.OnAppearingCommand.Execute(null);
    }
}