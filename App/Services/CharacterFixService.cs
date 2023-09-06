using App.Dtos;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Logging;

namespace App.Services;

public class CharacterFixService : ICharacterFixService
{
    private readonly ILogger<CharacterFixService> _logger;

    public CharacterFixService(ILogger<CharacterFixService> logger)
    {
        _logger = logger;
    }

    public IList<ParagraphDto> GetParagraphs(string filePath)
    {
        IList<ParagraphDto> result = new List<ParagraphDto>();

        using var doc = WordprocessingDocument.Open(filePath, false);
        var document = doc.MainDocumentPart.Document.Body;
        var paragraphs = document.Descendants<Paragraph>().Where(p => !string.IsNullOrEmpty(p.InnerText)).ToList();
        for (var i = 0; i < paragraphs.Count(); i++)
        {
            result.Add(new ParagraphDto(paragraphs[i].InnerText));
        }

        _logger.LogInformation($"Total of {paragraphs.Count()} paragraphs loaded from {filePath}");

        return result;
    }

    public void UpdateParagraphs(string filePath, IList<ParagraphDto> result)
    {
        using var doc = WordprocessingDocument.Open(filePath, true);
        var document = doc.MainDocumentPart.Document.Body;
        var paragraphs = document.Descendants<Paragraph>().Where(p => !string.IsNullOrEmpty(p.InnerText)).ToList();
        for (var i = 0; i < paragraphs.Count(); i++)
        {
            if (result[i].HasChanged)
            {
                paragraphs[i].InnerXml = paragraphs[i].InnerXml.Replace(result[i].OriginalText, result[i].FixedText);
            }
        }

        _logger.LogInformation($"Total of {paragraphs.Count()} paragraphs saved to {filePath}");
    }
}