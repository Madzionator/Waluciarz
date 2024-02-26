using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using Waluciarz.MVVM.Models;
using Waluciarz.Services;

namespace Waluciarz.MVVM.ViewModels;

public partial class ExchangeRateViewModel : ObservableObject
{
    private readonly IExchangeService _exchangeService;

    private (string, decimal)[] _current = Array.Empty<(string, decimal)>();

    [ObservableProperty]
    private List<Currency> _currencies;

    [ObservableProperty]
    private Currency _currencyFrom;

    [ObservableProperty]
    private Currency _currencyTo;

    [ObservableProperty]
    private ISeries[] _series;

    [ObservableProperty]
    private Axis[] _axes;

    [ObservableProperty]
    private string _actualRate;

    [ObservableProperty]
    private string _lowestRate;

    [ObservableProperty]
    private string _highestRate;

    [ObservableProperty]
    private string _avgIncreasePercent;

    public ExchangeRateViewModel(IExchangeService exchangeService)
    {
        _exchangeService = exchangeService;
        Task.Run(InitAsync);
    }

    private async Task InitAsync()
    {
        var currencies = await _exchangeService.GetAllAvailableCurrencies();
        Currencies = currencies
            .Select(x => new Currency(x.Key, x.Value))
            .ToList();
        CurrencyFrom = Currencies.FirstOrDefault(x => x.Symbol == "USD")!;
        CurrencyTo = Currencies.FirstOrDefault(x => x.Symbol == "PLN")!;
        await ChangeChart();
    }

    [RelayCommand]
    private async Task ChangeChart()
    {
        var data = await _exchangeService.GetCurrencyValues(CurrencyTo.Symbol, CurrencyFrom.Symbol);

        Series = new ISeries[]
        {
            new LineSeries<decimal>
            {
                Name = "Exchange Rate",
                Values = data.Select(x => x.Item2),
                TooltipLabelFormatter = (chartPoint) => $"{chartPoint.PrimaryValue}"
            }
        };

        Axes = new Axis[]
        {
            new Axis
            {
                Labels = data.Select(x => x.Item1).ToArray(),
                LabelsRotation = 0,
                SeparatorsAtCenter = false,
                TicksAtCenter = true
            }
        };

        var actual = Math.Round(data.Last().Item2, 4);
        var avg = data.Take(data.Length - 1).Select(x => x.Item2).Average();
        var avgInc = actual / avg * 100 - 100;

        ActualRate = $"{actual} {CurrencyTo.Symbol}";
        LowestRate = $"👇🏼 {Math.Round(data.MinBy(x => x.Item2).Item2, 4)} {CurrencyTo.Symbol}";
        HighestRate = $"👆🏼 {Math.Round(data.MaxBy(x => x.Item2).Item2, 4)} {CurrencyTo.Symbol}";
        AvgIncreasePercent = avgInc >= 0 ? $"+{Math.Round(avgInc, 4)}%" : $"{Math.Round(avgInc, 4)}%";
    }
}