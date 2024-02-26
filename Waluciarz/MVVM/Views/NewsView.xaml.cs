using CommunityToolkit.Mvvm.DependencyInjection;
using Waluciarz.MVVM.ViewModels;

namespace Waluciarz.MVVM.Views;

public partial class NewsView : ContentPage
{
	public NewsView()
	{
		InitializeComponent();

        BindingContext = Ioc.Default.GetRequiredService<NewsViewModel>();
    }
}