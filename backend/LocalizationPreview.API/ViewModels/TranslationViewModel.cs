namespace LocalizationPreview.API.ViewModels;

public class TranslationViewModel
{
    public long Id { get; set; }

    public long EntityId { get; set; }

    public string EntityName { get; set; }

    public string LanguageCode { get; set; }

    public Dictionary<string, string> TranslationFields { get; set; }
}