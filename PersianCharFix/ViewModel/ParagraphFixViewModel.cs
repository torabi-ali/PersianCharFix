using System.Collections.ObjectModel;

namespace PersianCharFix.ViewModel
{
    public class ParagraphFixViewModel
    {
        public string OldParagraph { get; set; }
        public int FixedParagraph { get; set; }
        public bool HasChanged { get; set; }

        public ObservableCollection<ParagraphFixViewModel> People { get; set; }
    }
}
