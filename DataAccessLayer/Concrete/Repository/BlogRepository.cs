using AutoMapper;
using BusinessLayer.Middlewares;
using Microsoft.EntityFrameworkCore;
using Shared.DTO.Blog.Request;
using Shared.DTO.Blog.Response;
using Shared.Interface;

namespace DataAccessLayer.Concrete.Repository;

public class BlogRepository : IBlogRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public BlogRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<Guid> DeleteBlog(Guid blogId)
    {
        var blog = await _applicationDbContext.Blogs
        .Where(x => x.Id == blogId)
        .FirstOrDefaultAsync()
        ?? throw new NotFoundException("Blog bulunamadı");

        _applicationDbContext.Blogs.Remove(blog);

        await _applicationDbContext.SaveChangesAsync();

        return blog.Id;
    }

    public async Task<BlogResponse> UpdateBlog(UpdateBlogRequest updateBlogRequest)
    {
        var blog = await _applicationDbContext.Blogs
        .Where(x => x.Id == updateBlogRequest.BlogId)
        .FirstOrDefaultAsync()
        ?? throw new NotFoundException("Blog bulunamadı");

        blog.Title = updateBlogRequest.Title;
        blog.LargeImageUrl = updateBlogRequest.LargeImgUrl;
        blog.SmallImageUrl = updateBlogRequest.SmallImgUrl;
        blog.Tags = updateBlogRequest.Tags;
        blog.Slug = updateBlogRequest.Slug;
        blog.OrderNumber = updateBlogRequest.OrderNumber;
        blog.Date = updateBlogRequest.Date;
        blog.IsActive = updateBlogRequest.IsActive;

        await _applicationDbContext.SaveChangesAsync();

        var blogResponse = _mapper.Map<BlogResponse>(blog);

        return blogResponse;
    }
}
