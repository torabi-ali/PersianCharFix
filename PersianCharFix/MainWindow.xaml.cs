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

            mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            mainWindow.ResizeMode = ResizeMode.NoResize;

            bw.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
            bw.WorkerReportsProgress = true;
        }

        public string CleanWord(string prg)
        {
            return prg.Trim().FixArabicChars().FixPersianChars().Fa2En();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "Open Word File ...";
            fileDialog.Filter = "Microsoft Word files|*.docx";
            fileDialog.InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString();

            var result = fileDialog.ShowDialog();

            if (result == true)
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

                try
                {
                    using (WordprocessingDocument doc = WordprocessingDocument.Open(fileDialog.FileName, true))
                    {
                        var document = doc.MainDocumentPart.Document.Body;
                        var prgTotal = document.Descendants<Paragraph>().Count();
                        var prgProgress = 0;

                        foreach (var paragraph in document.Descendants<Paragraph>())
                        {
                            if (paragraph.InnerText.Equals(""))
                                continue;

                            txtChecked = false;
                            string prgFixed = CleanWord(paragraph.InnerText);

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
                            var progress = (100 * prgProgress / prgTotal);
                            bw.ReportProgress(progress);
                        }
                    }
                    e.Result = FunctionResult.Done;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: Could not read file from disk.{Environment.NewLine}{ex.Message}");
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
                MessageBox.Show("It's successfully Done!", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
                progressBar.Value = 100;
            }
            else if (result == FunctionResult.Error)
            {
                MessageBox.Show("An Error has occurred. Please check the file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

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
