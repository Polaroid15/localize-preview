using LocalizationPreview.API.Features.Translations.CreateTranslation;
using LocalizationPreview.Core.Dto;
using LocalizationPreview.Core.Entities;
using LocalizationPreview.Core.Interfaces;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace LocalizationPreview.UnitTests.MediatrHandlers; 

public class CreateTranslationHandlerTest 
{
    private readonly Mock<ITranslationService> _mockTranslationService;
    private const long ReturningId = 1;

    public CreateTranslationHandlerTest() 
    {
        var translationFields = new Dictionary<string, string>{{"title", "el title"}};
        var translation = new Translation() 
        {
            Id = 2,
            EntityName = "exist entity name",
            LanguageCode = "exist lang code",
            EntityId = 10,
            TranslationFields = JObject.FromObject(translationFields)
        };
        _mockTranslationService = new Mock<ITranslationService>();
        _mockTranslationService.Setup(x => x.FindAsync(translation.EntityId, translation.EntityName, translation.LanguageCode))
            .ReturnsAsync(translation);
        _mockTranslationService.Setup(x => x.CreateAsync(It.IsAny<TranslationServiceDto>()))
            .ReturnsAsync(ReturningId);
    }

    [Fact]
    public async Task CorrectArgument_ReturnId() 
    {
        var command = new CreateTranslationCommand() 
        {
            EntityId = 10,
            EntityName = "test entity name",
            LanguageCode = "es",
            TranslationFields = new Dictionary<string, string> { { "title", "el title" } }
        };

        var handler = new CreateTranslationHandler(_mockTranslationService.Object);

        var result = await handler.Handle(command, default);
        
        Assert.True(result == ReturningId);
    }
    
    [Fact]
    public async Task CreateExistedItem_ReturnArgumentException() 
    {
        var command = new CreateTranslationCommand() 
        {
            EntityName = "exist entity name",
            LanguageCode = "exist lang code",
            EntityId = 10,
            TranslationFields = new Dictionary<string, string> { { "title", "el title" } }
        };

        var handler = new CreateTranslationHandler(_mockTranslationService.Object);

        var exception = await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, default));

        Assert.NotNull(exception);

        _mockTranslationService.Verify(x => x.FindAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
    }
}