using System.ComponentModel.DataAnnotations;
using LocalizationPreview.Core.Attributes;
using MediatR;

namespace LocalizationPreview.API.Features.Translations.UpdateTranslation;

public class UpdateTranslationCommand : IRequest<long>
{
    [Range(1, int.MaxValue)]
    public long Id { get; set; }

    [Range(1, int.MaxValue)]
    public long EntityId { get; set; }

    [Required]
    public string EntityName { get; set; }

    [Required]
    [SupportLanguage]
    public string LanguageCode { get; set; }

    [Required]
    public Dictionary<string, string> TranslationFields { get; set; }
}