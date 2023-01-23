namespace LocalizationPreview.Core.Dto;

public record IntegrationItemOutboxDto {
    public string NameItem { get; set; }
    public string Data { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime DeletionDate { get; set; }
}