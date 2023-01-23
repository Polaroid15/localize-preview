using System.Runtime.CompilerServices;
using LocalizationPreview.API.Features.Translations.GetTranslation;
using LocalizationPreview.Core.Entities;
using LocalizationPreview.Core.Interfaces;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace LocalizationPreview.UnitTests.MediatrHandlers; 

public class GetTranslationHandlerTest 
{
    private readonly Mock<ITranslationService> _mockTranslationService;

    public GetTranslationHandlerTest() 
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
        _mockTranslationService.Setup(x => x.FindAsync(1, It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(translation);
    }

    [Fact]
    public async Task CorrectArguments_ExpectTranslationViewModel() 
    {
        var query = new GetTranslationQuery() 
        {
            EntityId = 1,
            EntityName = "test entity name",
            LanguageCode = "es"
        };
        var handler = new GetTranslationHandler(_mockTranslationService.Object);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task WrongArguments_ExpectNull() {
        var query = new GetTranslationQuery() {
            EntityId = 2,
            EntityName = "test entity name",
            LanguageCode = "es"
        };
        var handler = new GetTranslationHandler(_mockTranslationService.Object);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.Null(result);
    }
}