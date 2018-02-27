using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Windows;

namespace PersianCharFix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static BackgroundWorker bw = new BackgroundWorker();
        public MainWindow()
        {
            InitializeComponent();

            mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            mainWindow.ResizeMode = ResizeMode.NoResize;

            bw.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
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
                try
                {
                    using (WordprocessingDocument doc = WordprocessingDocument.Open(fileDialog.FileName, true))
                    {
                        var document = doc.MainDocumentPart.Document;

                        foreach (var paragraph in document.Descendants<Paragraph>())
                        {
                            paragraph.InnerXml = paragraph.InnerXml.FixChars();
                        }
                    }
                    e.Result = FunctionResult.Done;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: Could not read file from disk. Original error: {ex.Message}");
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
    }
}
