using System.Windows;

namespace Wpf.Helpers;

public static class CloseCommand
{
    public static readonly DependencyProperty CloseCommandProperty =
        DependencyProperty.RegisterAttached(nameof(CloseCommand), typeof(bool?), typeof(CloseCommand), new PropertyMetadata(OnCloseCommandChanged));

    private static void OnCloseCommandChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is Window window)
        {
            window.Close();
        }
    }

    public static void SetCloseCommand(Window target, bool? value)
    {
        target.SetValue(CloseCommandProperty, value);
    }
}
