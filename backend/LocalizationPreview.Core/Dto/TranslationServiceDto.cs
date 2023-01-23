namespace LocalizationPreview.Core.Dto;

public record TranslationServiceDto {
    public long EntityId { get; set; }
    public string EntityName { get; set; }
    public string LanguageCode { get; set; }
    public Dictionary<string, string> TranslationFields { get; set; }
}