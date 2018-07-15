using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace PersianCharFix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static BackgroundWorker bw = new BackgroundWorker();
        private bool txtChecked = false;

        public MainWindow()
        {
            InitializeComponent();
            MainWindowInitial();

            bw.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
            bw.WorkerReportsProgress = true;
        }

        public void MainWindowInitial()
        {
            this.Dispatcher.Invoke(() =>
            {
                mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                mainWindow.ResizeMode = ResizeMode.NoResize;

                txtTitle.Text = "فایل خود را انتخاب کنید:";
                btnFileDialog.Visibility = Visibility.Visible;

                lblSource.Visibility = Visibility.Hidden;
                txtSource.Visibility = Visibility.Hidden;
                btnChecked.Visibility = Visibility.Hidden;
                lblFixed.Visibility = Visibility.Hidden;
                txtFixed.Visibility = Visibility.Hidden;
            });
        }

        public void MainWindowLivePreview()
        {
            this.Dispatcher.Invoke(() =>
            {
                txtTitle.Text = "تغییرات فایل‌ها را تأیید کنید:";
                btnFileDialog.Visibility = Visibility.Hidden;

                lblSource.Visibility = Visibility.Visible;
                txtSource.Visibility = Visibility.Visible;
                btnChecked.Visibility = Visibility.Visible;
                lblFixed.Visibility = Visibility.Visible;
                txtFixed.Visibility = Visibility.Visible;
            });
        }

        public string CleanParagraph(string prg)
        {
            return prg.Trim().FixArabicChars().FixPersianChars().Fa2En();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "انتخاب فایل";
            fileDialog.Filter = "Microsoft Word files|*.docx";
            fileDialog.InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString();

            var result = fileDialog.ShowDialog();

            if (result == true)
            {
                try
                {
                    using (WordprocessingDocument doc = WordprocessingDocument.Open(fileDialog.FileName, true))
                    {
                        MainWindowLivePreview();

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

                    //The following code will restart the app (if needed).
                    //System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                    //this.Dispatcher.Invoke(() =>
                    //{
                    //    Application.Current.Shutdown();
                    //});
                }
            }
            else
            {
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

            MainWindowInitial();
        }

        private void btnFileDialog_Click(object sender, RoutedEventArgs e)
        {
            if (!bw.IsBusy)
            {
                progressBar.Value = 0;
                bw.RunWorkerAsync();
            }
        }

        private void btnChecked_Click(object sender, RoutedEventArgs e)
        {
            txtChecked = true;
        }
    }
}
