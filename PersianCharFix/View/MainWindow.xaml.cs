using Microsoft.Win32;
using PersianCharFix.ViewModel;
using System;
using System.Windows;

namespace PersianCharFix.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            txtIntro.Text = "کارکترهای چندگانه و نویسه‌های غیرفارسی که سال‌ها در متون فارسی مخصوصاً در صفحات وب جا خوش کرده‌اند و مورد استفاده مجدد در تحقیقات علمی قرار گرفته‌اند.";
            txtIntro.Text += $"{Environment.NewLine}ما را بر آن داشت تا با ایجاد این برنامه به از بین بردن آن‌ها بپردازیم.";
        }

        private void btnFileDialog_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "انتخاب فایل";
            fileDialog.Filter = "Microsoft Word File|*.docx";
            fileDialog.InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString();

            var result = fileDialog.ShowDialog();

            if (result == true)
            {
                var charFixWindow = new CharFixWindow(new CharFixViewModel(fileDialog.FileName));
                charFixWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("در انتخاب فایل مشکلی رخ داده است. لطفاً دوباره امتحان کنید!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
