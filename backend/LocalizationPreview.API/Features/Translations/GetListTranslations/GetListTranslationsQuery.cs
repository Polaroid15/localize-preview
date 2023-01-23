using System.ComponentModel.DataAnnotations;
using LocalizationPreview.API.ViewModels;
using MediatR;

namespace LocalizationPreview.API.Features.Translations.GetListTranslations;

public class GetListTranslationsQuery : IRequest<IEnumerable<TranslationViewModel>>
{
    [Required]
    public string EntityName { get; set; }

    [Required]
    public string LanguageCode { get; set; }
}