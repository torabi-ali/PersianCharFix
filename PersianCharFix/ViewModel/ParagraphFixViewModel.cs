using PersianCharFix.Helpers;

namespace PersianCharFix.ViewModel
{
    public class ParagraphFixViewModel
    {
        public ParagraphFixViewModel(string oldParagraph)
        {
            OldText = oldParagraph;
            AutoFixedText = CleanParagraph(oldParagraph);

            if (AutoFixedText.Equals(OldText))
                HasChanged = false;
            else
                HasChanged = true;
        }

        public string OldText { get; set; }
        public string AutoFixedText { get; set; }
        public string FinalText { get; set; }
        public bool HasChanged { get; set; }

        public string CleanParagraph(string prg)
        {
            return prg.Trim().FixArabicChars().FixPersianChars().Fa2En();
        }
    }
}
