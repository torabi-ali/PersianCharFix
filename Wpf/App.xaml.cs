using App.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Windows;
using Wpf.ViewModels;
using Wpf.Views;

namespace Wpf;

public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; }

    public App()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();

        var currentDomain = AppDomain.CurrentDomain;
        currentDomain.UnhandledException += CurrentDomain_UnhandledException;
    }

    private void ConfigureServices(ServiceCollection services)
    {
        services.AddLogging(logBuilder =>
        {
            logBuilder.ClearProviders();
            logBuilder.AddNLog("NLog.config");
        });

        services.AddSingleton<ICharacterFixService, CharacterFixService>();

        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainViewModel>();

        services.AddSingleton<CharFixWindow>();
        services.AddSingleton<CharFixViewModel>();
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var logger = ServiceProvider.GetRequiredService<ILogger<App>>();

        var ex = (Exception)e.ExceptionObject;
        logger.LogError(ex, $"Error from sender: {sender}");
    }
}

