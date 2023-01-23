using LocalizationPreview.API.Features.Translations.GetListTranslations;
using LocalizationPreview.API.Features.Translations.GetTranslationById;
using LocalizationPreview.Core.Entities;
using LocalizationPreview.Core.Interfaces;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace LocalizationPreview.UnitTests.MediatrHandlers; 

public class GetListTranslationsHandlerTest 
{
    private readonly Mock<ITranslationService> _mockTranslationService;

    public GetListTranslationsHandlerTest() 
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
        _mockTranslationService.Setup(x => x.FindListAsync(It.IsAny<string>(), "es"))
            .ReturnsAsync(new List<Translation>() {translation});
    }

    [Fact]
    public async Task CorrectArguments_ExpectTranslationViewModel() 
    {
        var query = new GetListTranslationsQuery() { EntityName = "not important for test", LanguageCode = "es"};
        var handler = new GetListTranslationsHandler(_mockTranslationService.Object);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }
    
    [Fact]
    public async Task WrongLanguageCode_ExpectEmptyCollection() 
    {
        var query = new GetListTranslationsQuery() { EntityName = "not important for test", LanguageCode = "wrong code"};
        var handler = new GetListTranslationsHandler(_mockTranslationService.Object);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}