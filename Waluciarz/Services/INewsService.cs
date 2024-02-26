using Waluciarz.MVVM.Models;

namespace Waluciarz.Services;

public interface INewsService
{
    Task<List<NewsItem>> GetNews();
}