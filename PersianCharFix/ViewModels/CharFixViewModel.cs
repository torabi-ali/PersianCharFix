using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Win32;
using PersianCharFix.Helpers;
using PersianCharFix.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using static System.Environment;

namespace PersianCharFix.ViewModels
{
    public class CharFixViewModel : BaseViewModel
    {
        private string FilePath;

        #region ParagraphFix
        public ObservableCollection<ParagraphFix> Paragraphs { get; set; }
        #endregion

        #region Command
        public RelayCommand FixedCommand { get; set; }
        public RelayCommand HelpCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        #endregion

        public CharFixViewModel()
        {
            Paragraphs = new ObservableCollection<ParagraphFix>();

            FixedCommand = new RelayCommand(Fixed);
            HelpCommand = new RelayCommand(Help);
            CancelCommand = new RelayCommand(Cancel);


            var fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "انتخاب فایل";
            fileDialog.Filter = "Microsoft Word File|*.docx";
            fileDialog.InitialDirectory = Properties.Settings.Default.DefaultPath;

            var result = fileDialog.ShowDialog();

            if (result == true)
            {
                FilePath = fileDialog.FileName;
                using (WordprocessingDocument doc = WordprocessingDocument.Open(FilePath, false))
                {
                    var document = doc.MainDocumentPart.Document.Body;
                    var paragraphs = document.Descendants<Paragraph>().ToList();
                    for (int i = 0; i < paragraphs.Count(); i++)
                    {
                        if (paragraphs[i].InnerText.Equals(""))
                            continue;
                        Paragraphs.Add(new ParagraphFix(paragraphs[i].InnerText));
                    }
                }
            }
            else
            {
                MessageBox.Show("در انتخاب فایل مشکلی رخ داده است. لطفاً دوباره امتحان کنید!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Fixed(object parameter)
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(FilePath, true))
            {
                var document = doc.MainDocumentPart.Document.Body;
                var paragraphs = document.Descendants<Paragraph>().ToList();
                for (int i = 0; i < paragraphs.Count(); i++)
                {
                    if (paragraphs[i].InnerText.Equals(""))
                        continue;
                    if (Paragraphs[i].HasChanged)
                    {
                        paragraphs[i].InnerXml = paragraphs[i].InnerXml.Replace(Paragraphs[i].OriginalText, Paragraphs[i].FixedText);
                    }
                }
            }
            MessageBox.Show("تغییرات شما ذخیره شد", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Help(object parameter)
        {
            string path = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Help.html";
            if (File.Exists(path))
            {
                Process.Start(new ProcessStartInfo(path));
            }
            else
            {
                MessageBox.Show($"فایل راهنما یافت نشد!{NewLine}{path}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel(object parameter)
        {
            CloseWindow();
        }
    }
}
