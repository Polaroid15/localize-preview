using LocalizationPreview.Core.Dto;
using LocalizationPreview.Core.Interfaces;
using MediatR;

namespace LocalizationPreview.API.Features.Translations.UpdateTranslation;

public class UpdateTranslationHandler : IRequestHandler<UpdateTranslationCommand, long>
{
    private readonly ITranslationService _translationService;

    public UpdateTranslationHandler(ITranslationService translationService)
    {
        _translationService = translationService;
    }

    public async Task<long> Handle(UpdateTranslationCommand request, CancellationToken cancellationToken)
    {
        var model = new TranslationServiceDto()
        {
            EntityId = request.EntityId,
            EntityName = request.EntityName,
            LanguageCode = request.LanguageCode,
            TranslationFields = request.TranslationFields,
        };
        var id = await _translationService.UpdateAsync(request.Id, model);
        return id;
    }
}