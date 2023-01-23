using System.ComponentModel.DataAnnotations;
using LocalizationPreview.API.ViewModels;
using LocalizationPreview.Core.Attributes;
using MediatR;

namespace LocalizationPreview.API.Features.Translations.GetTranslation;

public class GetTranslationQuery : IRequest<TranslationViewModel>
{
    [Range(1, int.MaxValue)]
    public long EntityId { get; set; }

    [Required]
    public string EntityName { get; set; }

    [Required]
    [SupportLanguage]
    public string LanguageCode { get; set; }
}