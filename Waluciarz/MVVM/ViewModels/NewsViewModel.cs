using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Waluciarz.MVVM.Models;
using Waluciarz.Services;

namespace Waluciarz.MVVM.ViewModels;

public partial class NewsViewModel : ObservableObject
{
    private readonly INewsService _newsService;

    [ObservableProperty] 
    private List<NewsItem> _news = new ();

    [RelayCommand]
    async Task OpenLink(string link)
    {
        await Browser.Default.OpenAsync(link);
    }

    public NewsViewModel(INewsService newsService)
    {
        _newsService = newsService;
        Task.Run(InitAsync);
    }

    public async Task InitAsync()
    {
        News = await _newsService.GetNews();
    }
}