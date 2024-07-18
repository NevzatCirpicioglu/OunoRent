namespace Shared.DTO.Category.Response;

public class CategoryResponse : GenericResponse
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
}