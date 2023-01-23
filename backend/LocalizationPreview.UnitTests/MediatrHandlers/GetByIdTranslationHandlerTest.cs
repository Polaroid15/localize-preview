using LocalizationPreview.API.Features.Translations.GetTranslationById;
using LocalizationPreview.Core.Entities;
using LocalizationPreview.Core.Interfaces;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace LocalizationPreview.UnitTests.MediatrHandlers; 

public class GetByIdTranslationHandlerTest 
{
    private readonly Mock<ITranslationService> _mockTranslationService;

    public GetByIdTranslationHandlerTest() 
    {
        var translationFields = new Dictionary<string, string>{{"title", "el title"}};
        var translation = new Translation() 
        {
            Id = 1,
            EntityName = "test entity name",
            LanguageCode = "es",
            EntityId = 10,
            TranslationFields = JObject.FromObject(translationFields)
        };
        _mockTranslationService = new Mock<ITranslationService>();
        _mockTranslationService.Setup(x => x.FindByIdAsync(translation.Id)).ReturnsAsync(translation);
    }

    [Fact]
    public async Task CorrectArguments_ExpectTranslationViewModel() 
    {
        var query = new GetTranslationByIdQuery() { Id = 1 };
        var handler = new GetTranslationByIdHandler(_mockTranslationService.Object);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task WrongId_ExpectNull() 
    {
        var query = new GetTranslationByIdQuery() { Id = 3 };
        var handler = new GetTranslationByIdHandler(_mockTranslationService.Object);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.Null(result);
    }
}