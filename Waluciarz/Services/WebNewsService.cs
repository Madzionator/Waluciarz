using System.Text.RegularExpressions;
using System.Xml.Linq;
using Waluciarz.MVVM.Models;

namespace Waluciarz.Services;


internal class WebNewsService : INewsService
{
    private const string RegexPattern = @".*(src="")(?<url>.*)("" alt.).*/>(?<text>.*)</p.*";
    private List<NewsItem> _news;

    public async Task<List<NewsItem>> GetNews()
    {
        using var client = new HttpClient();
        var xml = await client.GetStringAsync("https://www.bankier.pl/rss/wiadomosci.xml");

        return XDocument.Parse(xml)
            .Descendants("item")
            .Select(item =>
            {
                var match = Regex.Match(item.Element("description")?.Value, RegexPattern);
                var imageUrl = match.Groups["url"];
                var text = match.Groups["text"];

                return new NewsItem
                {
                    Title = item.Element("title")?.Value,
                    Link = item.Element("link")?.Value,
                    Description = text.Value,
                    ImageUrl = imageUrl.Value,
                    PubDate = DateTime.TryParse(item.Element("pubDate")?.Value, out var pubDate)
                        ? pubDate
                        : DateTime.MinValue
                };
            })
            .ToList();
    }
}