using Waluciarz.MVVM.Models;

namespace Waluciarz.Services;

public interface IExchangeService
{
    Task<CurrencySymbols> GetAllAvailableCurrencies();
    Task<(string, decimal)[]> GetCurrencyValues(string symbol, string currencyBase);
}