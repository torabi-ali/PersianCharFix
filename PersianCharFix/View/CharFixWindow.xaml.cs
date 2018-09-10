using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using PersianCharFix.Helpers;
using PersianCharFix.Model;
using PersianCharFix.ViewModel;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace PersianCharFix.View
{
    /// <summary>
    /// Interaction logic for CharFixWindow.xaml
    /// </summary>
    public partial class CharFixWindow : Window
    {
        private static BackgroundWorker bw = new BackgroundWorker();
        private bool txtChecked = false;
        private CharFixViewModel _CharFixViewModel;

        public CharFixWindow(CharFixViewModel charFixViewModel)
        {
            InitializeComponent();
            _CharFixViewModel = charFixViewModel;
            charFixWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            charFixWindow.ResizeMode = ResizeMode.NoResize;

            bw.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
            bw.WorkerReportsProgress = true;

            bw.RunWorkerAsync();
        }

        public string CleanParagraph(string prg)
        {
            return prg.Trim().FixArabicChars().FixPersianChars().Fa2En();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (WordprocessingDocument doc = WordprocessingDocument.Open(_CharFixViewModel.FilePath, true))
                {
                    var document = doc.MainDocumentPart.Document.Body;
                    var prgTotal = document.Descendants<Paragraph>().Count();
                    var prgProgress = 0;

                    foreach (var paragraph in document.Descendants<Paragraph>())
                    {
                        if (paragraph.InnerText.Equals(""))
                            continue;

                        txtChecked = false;
                        string prgFixed = CleanParagraph(paragraph.InnerText);

                        this.Dispatcher.Invoke(() =>
                        {
                            txtSource.Text = paragraph.InnerText;
                            txtFixed.Text = prgFixed;
                        });

                        while (!txtChecked)
                        {
                            System.Threading.Thread.Sleep(100);
                        }

                        this.Dispatcher.Invoke(() =>
                        {
                            paragraph.InnerXml = paragraph.InnerXml.Replace(paragraph.InnerText, txtFixed.Text);
                        });

                        prgProgress++;
                        bw.ReportProgress(100 * prgProgress / prgTotal);
                    }
                }
                e.Result = FunctionResult.Done;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                e.Result = FunctionResult.Error;
            }
        }

        public void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        public void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var result = (FunctionResult)e.Result;

            if (result == FunctionResult.Done)
            {
                progressBar.Value = 100;
                MessageBox.Show("اصلاحات در فایل ذخیره شد", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (result == FunctionResult.Error)
            {
                MessageBox.Show("خطایی رخ داده است. لطفاً فایل خود را بررسی کنید", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void btnChecked_Click(object sender, RoutedEventArgs e)
        {
            txtChecked = true;
        }
    }
}
