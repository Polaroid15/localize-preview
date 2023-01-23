using System.ComponentModel.DataAnnotations;
using LocalizationPreview.API.ViewModels;
using MediatR;

namespace LocalizationPreview.API.Features.Translations.GetTranslationById;

public class GetTranslationByIdQuery : IRequest<TranslationViewModel>
{
    [Range(1, int.MaxValue)]
    public long Id { get; set; }
}