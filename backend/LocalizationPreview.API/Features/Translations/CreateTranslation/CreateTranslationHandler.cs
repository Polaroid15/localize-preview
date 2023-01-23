using LocalizationPreview.Core.Dto;
using LocalizationPreview.Core.Interfaces;
using MediatR;

namespace LocalizationPreview.API.Features.Translations.CreateTranslation;

public class CreateTranslationHandler : IRequestHandler<CreateTranslationCommand, long>
{
    private readonly ITranslationService _translationService;

    public CreateTranslationHandler(ITranslationService translationService)
    {
        _translationService = translationService;
    }

    public async Task<long> Handle(CreateTranslationCommand request, CancellationToken cancellationToken)
    {
        var model = new TranslationServiceDto()
        {
            EntityId = request.EntityId,
            EntityName = request.EntityName,
            LanguageCode = request.LanguageCode,
            TranslationFields = request.TranslationFields,
        };
        var id = await _translationService.CreateAsync(model);
        return id;
    }
}