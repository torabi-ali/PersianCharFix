using App.Dtos;

namespace App.Services;

public interface ICharacterFixService
{
    IList<ParagraphDto> GetParagraphs(string filePath);

    void UpdateParagraphs(string filePath, IList<ParagraphDto> paragraphs);
}