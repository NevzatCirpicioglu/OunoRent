using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Entities;

public class Category : AuditTrailer
{
    [Key]
    public Guid CategoryId { get; set; }

    public string Name { get; set; }

    // Relationships

    public ICollection<SubCategory> SubCategories { get; set; }
}