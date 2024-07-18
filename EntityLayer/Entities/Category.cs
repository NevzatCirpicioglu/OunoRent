namespace EntityLayer.Entities;

public class Category : AuditTrailer
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public ICollection<Product> Products { get; set; }
}