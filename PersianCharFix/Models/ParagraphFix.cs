using PersianCharFix.Utility;

namespace PersianCharFix.Models
{
    public class ParagraphFix
    {
        public string OriginalText { get; set; }
        public string FixedText { get; set; }
        public bool HasChanged { get; set; }

        public ParagraphFix(string _originalText)
        {
            OriginalText = _originalText;
            FixedText = OriginalText.CleanString();

            if (OriginalText.Equals(FixedText))
                HasChanged = false;
            else
                HasChanged = true;
        }
    }
}
