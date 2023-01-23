using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;

namespace LocalizationPreview.Core.Entities; 

[Table("translations")]
public class Translation : BaseEntity {
    
    [Column("id")]
    public long EntityId { get; set; }
    
    [Column("entity_name")]
    public string EntityName { get; set; }
    
    [Column("language_code")]
    public string LanguageCode { get; set; }
    
    [Column("translation_fields")]
    public JObject TranslationFields { get; set; }
}