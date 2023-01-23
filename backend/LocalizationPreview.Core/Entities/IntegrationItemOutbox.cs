using System.ComponentModel.DataAnnotations.Schema;

namespace LocalizationPreview.Core.Entities; 

[Table("integration_items_outbox")]
public class IntegrationItemOutbox : BaseEntity {
    
    [Column("name_item")]
    public string NameItem { get; set; }
    
    [Column("data")]
    public string Data { get; set; }
    
    [Column("is_deleted")]
    public bool IsDeleted { get; set; }
    
    [Column("deletion_date")]
    public DateTimeOffset DeletionDate { get; set; }
}