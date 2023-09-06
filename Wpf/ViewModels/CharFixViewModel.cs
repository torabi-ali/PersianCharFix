using App.Dtos;
using App.Services;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using Wpf.Helpers;

namespace Wpf.ViewModels
{
    public class CharFixViewModel : BaseViewModel
    {
        private string _filePath;
        private readonly ICharacterFixService _characterFixService;

        public static SnackbarMessageQueue MessageQueue { get; set; }

        public ObservableCollection<ParagraphDto> Paragraphs { get; private set; }

        public string FixActionContent => Paragraphs.Any() ? "تبدیل" : "بارگذاری";

        public RelayCommand FixActionCommand { get; set; }
        public RelayCommand HelpCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public CharFixViewModel()
        {
            _characterFixService = App.ServiceProvider.GetRequiredService<ICharacterFixService>();
            MessageQueue = new SnackbarMessageQueue();

            FixActionCommand = new RelayCommand(LoadOrUpdateFile);
            HelpCommand = new RelayCommand(Help);
            CancelCommand = new RelayCommand(Cancel);

            Initialize();
        }

        public void Initialize()
        {
            Paragraphs = new ObservableCollection<ParagraphDto>();
            _filePath = null;

            RaisePropertyChanged(nameof(Paragraphs));
            RaisePropertyChanged(nameof(FixActionContent));
        }

        private void LoadFile()
        {
            var fileDialog = new OpenFileDialog { Multiselect = false, Title = "انتخاب فایل", Filter = "Microsoft Word File|*.docx" };

            var result = fileDialog.ShowDialog();
            if (result == true)
            {
                _filePath = fileDialog.FileName;
                var paragraphs = _characterFixService.GetParagraphs(_filePath);
                foreach (var item in paragraphs)
                {
                    Paragraphs.Add(item);
                }

                RaisePropertyChanged(nameof(FixActionContent));
            }
            else
            {
                MessageQueue.Enqueue("در انتخاب فایل مشکلی رخ داده است. لطفاً دوباره امتحان کنید!");
            }
        }

        private void LoadOrUpdateFile(object parameter)
        {
            if (!Paragraphs.Any())
            {
                LoadFile();

                MessageQueue.Enqueue("بارگذاری انجام شد");
            }
            else
            {
                _characterFixService.UpdateParagraphs(_filePath, Paragraphs);
                Initialize();

                MessageQueue.Enqueue("تغییرات شما ذخیره شد");
            }
        }

        private void Help(object parameter)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Help.html"),
                UseShellExecute = true
            });
        }

        private void Cancel(object parameter)
        {
            CloseWindow();
        }
    }
}
