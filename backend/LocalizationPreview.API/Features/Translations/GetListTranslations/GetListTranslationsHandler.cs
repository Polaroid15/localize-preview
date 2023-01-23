using LocalizationPreview.API.ViewModels;
using LocalizationPreview.Core.Interfaces;
using MediatR;
using Newtonsoft.Json.Linq;

namespace LocalizationPreview.API.Features.Translations.GetListTranslations;

public class GetListTranslationsHandler : IRequestHandler<GetListTranslationsQuery, IEnumerable<TranslationViewModel>>
{
    private readonly ITranslationService _translationService;

    public GetListTranslationsHandler(ITranslationService translationService)
    {
        _translationService = translationService;
    }

    public async Task<IEnumerable<TranslationViewModel>> Handle(GetListTranslationsQuery request, CancellationToken cancellationToken)
    {
        var translations = await _translationService.FindListAsync(request.EntityName, request.LanguageCode);
        if (translations == null || !translations.Any())
        {
            return new List<TranslationViewModel>();
        }

        var result = translations.Select(translation => new TranslationViewModel()
        {
            Id = translation.Id,
            EntityId = translation.EntityId,
            EntityName = translation.EntityName,
            LanguageCode = translation.LanguageCode,
            TranslationFields = JObject.FromObject(translation.TranslationFields).ToObject<Dictionary<string, string>>(),
        }).ToList();
        return result;
    }
}