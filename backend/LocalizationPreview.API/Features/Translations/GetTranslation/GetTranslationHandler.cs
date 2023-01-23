using LocalizationPreview.API.ViewModels;
using LocalizationPreview.Core.Interfaces;
using MediatR;
using Newtonsoft.Json.Linq;

namespace LocalizationPreview.API.Features.Translations.GetTranslation;

public class GetTranslationHandler : IRequestHandler<GetTranslationQuery, TranslationViewModel>
{
    private readonly ITranslationService _translationService;

    public GetTranslationHandler(ITranslationService translationService)
    {
        _translationService = translationService;
    }

    public async Task<TranslationViewModel> Handle(GetTranslationQuery request, CancellationToken cancellationToken)
    {
        var translation = await _translationService.FindAsync(request.EntityId, request.EntityName, request.LanguageCode);
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