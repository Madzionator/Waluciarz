using CommunityToolkit.Mvvm.DependencyInjection;
using Waluciarz.MVVM.ViewModels;

namespace Waluciarz.MVVM.Views;

public partial class ExchangeRateView : ContentPage
{
	public ExchangeRateView()
	{
		InitializeComponent();
        BindingContext = Ioc.Default.GetRequiredService<ExchangeRateViewModel>();
    }
}