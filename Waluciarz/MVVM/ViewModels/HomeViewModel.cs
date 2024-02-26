using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Waluciarz.MVVM.Views;

namespace Waluciarz.MVVM.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly INavigation _navigation;

    public HomeViewModel(INavigation navigation)
    {
        _navigation = navigation;
    }

    [RelayCommand]
    async Task NavigateExchangeRate()
    {
        await _navigation.PushAsync(new ExchangeRateView());
    }

    [RelayCommand]
    async Task NavigateWorldCurrencies()
    {
        await _navigation.PushAsync(new NewsView());
    }
}