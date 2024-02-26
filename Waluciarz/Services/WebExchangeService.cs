using System.Globalization;
using System.Net.Http.Json;
using Waluciarz.MVVM.Models;

namespace Waluciarz.Services;

internal class WebExchangeService : IExchangeService, IDisposable
{
    private const string UrlBase = "https://api.apilayer.com/fixer";
    private const string DateFormat = "yyyy-MM-dd";
    private const string ChartDateFormat = "dd-MM";
    private const int Days = 14;
    
    private readonly HttpClient _client = new();

    public WebExchangeService()
    {
        _client.DefaultRequestHeaders.Add("apikey", "add-your-key"); //necessary for exchange rates to work !!!
    }

    public async Task<CurrencySymbols> GetAllAvailableCurrencies()
    {
        var data = await _client.GetFromJsonAsync<CurrencySymbolsDto>($"{UrlBase}/symbols");
        return data!.Symbols;
    }

    public async Task<(string, decimal)[]> GetCurrencyValues(string symbol, string currencyBase)
    {
        /*return new[]
        {
            ("26-04", 4.151404m),
            ("27-04", 4.147144m),
            ("28-04", 4.162526m),
            ("29-04", 4.162526m),
            ("30-04", 4.169495m),
            ("01-05", 4.199008m),
            ("02-05", 4.157681m),
            ("03-05", 4.146065m),
            ("04-05", 4.164663m),
            ("05-05", 4.153428m),
            ("06-05", 4.153071m),
            ("07-05", 4.152437m),
            ("08-05", 4.148262m),
            ("09-05", 4.152549m),
        };*/

        var today = DateTime.Now.Date;

        if (symbol.Equals(currencyBase, StringComparison.OrdinalIgnoreCase))
        {
            return Enumerable
                .Range(1 - Days, Days)
                .Select(i => (today.AddDays(i).ToString(ChartDateFormat), 1m))
                .ToArray();
        }

        var parameters = new Dictionary<string,string>()
        {
            {"start_date", today.AddDays(1 - Days).ToString(DateFormat) },
            {"end_date", today.ToString(DateFormat)},
            {"base", currencyBase},
            {"symbols", symbol}
        };

        var query = string.Join('&', parameters.Select(q => $"{q.Key}={q.Value}"));
        var data = await _client.GetFromJsonAsync<ExchangeRateDto>($"{UrlBase}/timeseries?{query}");

        var res = data.Rates
            .OrderBy(x => x.Key)
            .Select(x => (DateTime.ParseExact(x.Key, DateFormat, CultureInfo.InvariantCulture)
                .ToString(ChartDateFormat), x.Value[symbol]))
            .ToArray();

        return res;
    }

    public void Dispose()
    {
        _client?.Dispose();
    }

    private class CurrencySymbolsDto
    {
        public bool Success { get; set; }
        public CurrencySymbols Symbols { get; set; }
    }

    public class ExchangeRateDto
    {
        public string Base { get; set; }
        public string EndDate { get; set; }
        public Dictionary<string, Dictionary<string, decimal>> Rates { get; set; }
        public string StartDate { get; set; }
        public bool Success { get; set; }
        public bool TimeSeries { get; set; }
    }
}