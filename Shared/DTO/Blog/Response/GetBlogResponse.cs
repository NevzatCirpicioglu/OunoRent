namespace Shared.DTO.Blog.Response;

public class GetBlogResponse : GenericResponse
{
    public Guid BlogId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string LargeImageUrl { get; set; }
    public string SmallImageUrl { get; set; }
    public string Tags { get; set; }
    public string Slug { get; set; }
    public int Order { get; set; }
    public DateTime Date { get; set; }
    public bool IsActive { get; set; }
}
