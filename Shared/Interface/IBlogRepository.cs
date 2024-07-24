using Shared.DTO.Blog.Request;
using Shared.DTO.Blog.Response;

namespace Shared.Interface;

public interface IBlogRepository
{
    Task<Guid> DeleteBlog(Guid blogId);
    Task<BlogResponse> UpdateBlog(UpdateBlogRequest updateBlogRequest);
}
