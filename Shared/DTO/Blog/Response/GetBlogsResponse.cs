namespace Shared.DTO.Blog.Response;

public class GetBlogsResponse
{
    public Guid BlogId { get; set; }
    public string Title { get; set; }
    public string Tags { get; set; }
    public string Slug { get; set; }
    public int Order { get; set; }
    public DateTime Date { get; set; }
    public bool IsActive { get; set; }
}
