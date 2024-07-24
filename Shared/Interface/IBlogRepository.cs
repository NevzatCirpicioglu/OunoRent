using Shared.DTO.Blog.Request;
using Shared.DTO.Blog.Response;

namespace Shared.Interface;

public interface IBlogRepository
{
    Task<BlogResponse> CreateBlogAsync(CreateBlogRequest createBlogRequest);
    Task<GetBlogResponse> GetBlogAsync(Guid id);
    Task<List<GetBlogsResponse>> GetBlogsAsync();
}
