namespace PersianCharFix.ViewModel
{
    public class CharFixViewModel
    {
        public CharFixViewModel(string filePath)
        {
            FilePath = filePath;
        }

        public string FilePath { get; set; }
    }
}
