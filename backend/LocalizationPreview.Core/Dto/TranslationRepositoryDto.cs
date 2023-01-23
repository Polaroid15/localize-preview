using Newtonsoft.Json.Linq;

namespace LocalizationPreview.Core.Dto;

public record TranslationRepositoryDto {
    public long EntityId { get; set; }
    public string EntityName { get; set; }
    public string LanguageCode { get; set; }
    public JObject TranslationFields { get; set; }
}