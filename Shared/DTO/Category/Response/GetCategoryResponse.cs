namespace Shared.DTO.Category.Response;

public class GetCategoryResponse : GenericResponse
{
    public Guid CategoryId { get; set; }

    public string CategoryName { get; set; }
}