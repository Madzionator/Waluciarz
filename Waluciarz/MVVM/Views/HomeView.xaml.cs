using HomeViewModel = Waluciarz.MVVM.ViewModels.HomeViewModel;

namespace Waluciarz.MVVM.Views;

public partial class HomeView : ContentPage
{
	public HomeView()
	{
		InitializeComponent();
        BindingContext = new HomeViewModel(Navigation);
    }
}