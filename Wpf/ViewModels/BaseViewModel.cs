using System.ComponentModel;

namespace Wpf.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private bool? _closeWindowFlag;

    internal void RaisePropertyChanged(string prop)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    public bool? CloseWindowFlag
    {
        get => _closeWindowFlag; set
        {
            _closeWindowFlag = value;
            RaisePropertyChanged(nameof(CloseWindowFlag));
        }
    }

    public virtual void CloseWindow(bool? result = true)
    {
        CloseWindowFlag = CloseWindowFlag is null ? true : !CloseWindowFlag;
    }
}
