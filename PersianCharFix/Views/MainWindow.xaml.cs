using System.Windows;
using static System.Environment;

namespace PersianCharFix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Initialization();
            InitializeComponent();

            txtIntro.Text = "کارکترهای چندگانه و نویسه‌های غیرفارسی که سال‌ها در متون فارسی مخصوصاً در صفحات وب جا خوش کرده‌اند و مورد استفاده مجدد در تحقیقات علمی قرار گرفته‌اند.";
            txtIntro.Text += $"{NewLine}ما را بر آن داشت تا با ایجاد این برنامه به از بین بردن آن‌ها بپردازیم.";
            txtIntro.Text += $"{NewLine}فایل خود را انتخاب کنید:";
        }

        public void Initialization()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.DefaultPath))
            {
                Properties.Settings.Default.DefaultPath = GetFolderPath(SpecialFolder.Desktop);
            }

            Properties.Settings.Default.Save();
        }
    }
}
