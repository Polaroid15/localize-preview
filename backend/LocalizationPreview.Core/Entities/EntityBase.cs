using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LocalizationPreview.Shared;

namespace LocalizationPreview.Core.Entities; 

public abstract class BaseEntity : IBaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public long Id { get; set; }

    [Column("created_date")]
    public virtual DateTimeOffset CreatedDate { get; set; }

    [Column("updated_date")]
    public virtual DateTimeOffset UpdatedDate { get; set; }
}