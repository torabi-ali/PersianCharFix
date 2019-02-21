using PersianCharFix.Helpers;

namespace PersianCharFix.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Command
        public RelayCommand CharFixCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        #endregion

        public MainViewModel()
        {
            CharFixCommand = new RelayCommand(CharFix);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void CharFix(object parameter)
        {
            var charFixWindow = new CharFixWindow();
            charFixWindow.Show();

            CloseWindow();
        }

        private void Cancel(object parameter)
        {
            CloseWindow();
        }
    }
}
