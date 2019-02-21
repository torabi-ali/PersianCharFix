using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace PersianCharFix.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool? _CloseWindowFlag;
        private double _CurrentProgress;

        internal void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public bool? CloseWindowFlag
        {
            get { return _CloseWindowFlag; }
            set
            {
                _CloseWindowFlag = value;
                RaisePropertyChanged("CloseWindowFlag");
            }
        }

        public double CurrentProgress
        {
            get { return _CurrentProgress; }
            set
            {
                _CurrentProgress = value;
                RaisePropertyChanged("CurrentProgress");
            }
        }

        public virtual void CloseWindow(bool? result = true)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                CloseWindowFlag = CloseWindowFlag == null ? true : !CloseWindowFlag;
            }));
        }
    }
}
