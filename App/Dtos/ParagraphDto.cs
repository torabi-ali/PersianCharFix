using App.Utility;

namespace App.Dtos;

public class ParagraphDto
{
    public string OriginalText { get; set; }

    public string FixedText { get; set; }

    public bool HasChanged { get; set; }

    public ParagraphDto(string originalText)
    {
        OriginalText = originalText;
        FixedText = OriginalText.CleanString();

        if (OriginalText.Equals(FixedText))
        {
            HasChanged = false;
        }
        else
        {
            HasChanged = true;
        }
    }
}

