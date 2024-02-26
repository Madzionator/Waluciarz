using Newtonsoft.Json;
using Waluciarz.MVVM.Models;

namespace Waluciarz.Services;

internal class CachedExchangeService : IExchangeService
{
    private readonly IExchangeService _decoratedService;
    private readonly FileInfo _file;

    public CachedExchangeService(IExchangeService decoratedService)
    {
        _decoratedService = decoratedService;

        var path = Path.Combine(FileSystem.Current.CacheDirectory, "currencySymbols.json");
        _file = new FileInfo(path);
    }

    public async Task<CurrencySymbols> GetAllAvailableCurrencies()
    {
        if (_file.Exists)
        {
            try
            {
                var json = await File.ReadAllTextAsync(_file.FullName);
                var data = JsonConvert.DeserializeObject<CurrencySymbols>(json);

                return data;
            }
            catch
            {
                // if failed, request again
            }
        }

        {
            var data = await _decoratedService.GetAllAvailableCurrencies();
            var json = JsonConvert.SerializeObject(data);

            await File.WriteAllTextAsync(_file.FullName, json);
            return data;
        }
    }

    public Task<(string, decimal)[]> GetCurrencyValues(string symbol, string currencyBase) => _decoratedService.GetCurrencyValues(symbol, currencyBase);
}