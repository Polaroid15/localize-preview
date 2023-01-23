using LocalizationPreview.API.ViewModels;
using LocalizationPreview.Core.Interfaces;
using MediatR;
using Newtonsoft.Json.Linq;

namespace LocalizationPreview.API.Features.Translations.GetTranslationById;

public class GetTranslationByIdHandler : IRequestHandler<GetTranslationByIdQuery, TranslationViewModel>
{
    private readonly ITranslationService _translationService;

    public GetTranslationByIdHandler(ITranslationService translationService)
    {
        _translationService = translationService;
    }

    public async Task<TranslationViewModel> Handle(GetTranslationByIdQuery request, CancellationToken cancellationToken)
    {
        var translation = await _translationService.FindByIdAsync(request.Id);
        if (translation == null)
        {
            return null;
        }

        var viewModel = new TranslationViewModel()
        {
            Id = translation.Id,
            EntityId = translation.EntityId,
            EntityName = translation.EntityName,
            LanguageCode = translation.LanguageCode,
            TranslationFields = JObject.FromObject(translation.TranslationFields).ToObject<Dictionary<string, string>>(),
        };
        return viewModel;
    }
}