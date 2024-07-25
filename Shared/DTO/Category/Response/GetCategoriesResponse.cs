namespace Shared.DTO.Category.Response;

public class GetCategoriesResponse : GenericResponse
{
    public Guid CategoryId { get; set; }

    public string Name { get; set; }

}