using Microsoft.Extensions.DependencyInjection;
using Wpf.Helpers;
using Wpf.Views;

namespace Wpf.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public RelayCommand CharFixCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public MainViewModel()
        {
            CharFixCommand = new RelayCommand(CharFix);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void CharFix(object parameter)
        {
            var charFixWindow = App.ServiceProvider.GetRequiredService<CharFixWindow>();
            charFixWindow.Show();

            CloseWindow();
        }

        private void Cancel(object parameter)
        {
            CloseWindow();
        }
    }
}
