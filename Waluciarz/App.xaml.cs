using Waluciarz.MVVM.Views;

namespace Waluciarz;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new NavigationPage(new HomeView());
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);

        const int newWidth = 576;
        const int newHeight = 1024;

        window.Width = newWidth;
        window.Height = newHeight;

        return window;
    }
}