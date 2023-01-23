using LocalizationPreview.API.Features.Translations.UpdateTranslation;
using LocalizationPreview.Core.Dto;
using LocalizationPreview.Core.Entities;
using LocalizationPreview.Core.Interfaces;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace LocalizationPreview.UnitTests.MediatrHandlers; 

public class UpdateTranslationHandlerTest 
{
    private readonly Mock<ITranslationService> _mockTranslationService;
    private const long UpdatingId = 1;

    public UpdateTranslationHandlerTest()
    {
        var translationFields = new Dictionary<string, string>{{"title", "old el title"}};
        var translation = new Translation() 
        {
            Id = UpdatingId,
            EntityName = "old entity name",
            LanguageCode = "es",
            EntityId = 10,
            TranslationFields = JObject.FromObject(translationFields)
        };
        _mockTranslationService = new Mock<ITranslationService>();
        _mockTranslationService.Setup(x => x.FindByIdAsync(UpdatingId)).ReturnsAsync(translation);
        _mockTranslationService.Setup(x => x.UpdateAsync(UpdatingId, It.IsAny<TranslationServiceDto>()))
            .ReturnsAsync(UpdatingId);
    }

    [Fact]
    public async Task CorrectArgument_ReturnId() 
    {
        var command = new UpdateTranslationCommand() 
        {
            Id = UpdatingId,
            EntityName = "new entity name",
            LanguageCode = "new lang code",
            EntityId = 10,
            TranslationFields = new Dictionary<string, string> { { "title", "new el title" } }
        };

        var handler = new UpdateTranslationHandler(_mockTranslationService.Object);

        var result = await handler.Handle(command, default);
        
        Assert.True(result == UpdatingId);
    }
    
    [Fact]
    public async Task UpdateNotExistedItem_ReturnArgumentException() 
    {
        var command = new UpdateTranslationCommand() 
        {
            Id = 200,
            EntityName = "e entity name",
            LanguageCode = "e lang code",
            EntityId = 10,
            TranslationFields = new Dictionary<string, string> { { "title", "el title" } }
        };

        var handler = new UpdateTranslationHandler(_mockTranslationService.Object);

        var exception = await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, default));

        Assert.NotNull(exception);

        _mockTranslationService.Verify(x => x.FindByIdAsync(It.IsAny<long>()), Times.Once());
    }
}