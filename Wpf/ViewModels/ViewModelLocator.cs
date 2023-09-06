using Microsoft.Extensions.DependencyInjection;

namespace Wpf.ViewModels;

public class ViewModelLocator
{
    public MainViewModel MainViewModel => App.ServiceProvider.GetRequiredService<MainViewModel>();

    public CharFixViewModel CharFixViewModel => App.ServiceProvider.GetRequiredService<CharFixViewModel>();
}
