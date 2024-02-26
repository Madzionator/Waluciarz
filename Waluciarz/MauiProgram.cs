using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Waluciarz.MVVM.ViewModels;
using Waluciarz.Services;

namespace Waluciarz;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        builder
            .UseSkiaSharp(true)
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services
            .AddScoped<WebExchangeService>()
            .AddScoped<IExchangeService>(provider => new CachedExchangeService(provider.GetService<WebExchangeService>()))
            .AddScoped<ExchangeRateViewModel>()
            .AddScoped<INewsService, WebNewsService>()
            .AddScoped<NewsViewModel>();
        
#if DEBUG
		builder.Logging.AddDebug();
#endif

        var app = builder.Build();
        Ioc.Default.ConfigureServices(app.Services);

        return app;
    }
}
