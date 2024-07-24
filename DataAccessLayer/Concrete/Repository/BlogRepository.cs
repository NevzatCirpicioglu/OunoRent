using System.Linq.Expressions;
using System.Reflection.Metadata;
using AutoMapper;
using BusinessLayer.Middlewares;
using EntityLayer.Entities;
using Ganss.Xss;
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

    public async Task<BlogResponse> CreateBlogAsync(CreateBlogRequest createBlogRequest)
    {
        var sanitizer = new HtmlSanitizer();
        var blog = _mapper.Map<Blog>(createBlogRequest);

        blog.Body = sanitizer.Sanitize(blog.Body);
        blog.Title = sanitizer.Sanitize(blog.Title);
        blog.Tags = sanitizer.Sanitize(blog.Tags);
        blog.Slug = sanitizer.Sanitize(blog.Slug);
        blog.IsActive = true;
        blog.Date = DateTime.UtcNow;

        var result = await _applicationDbContext.Blogs.AddAsync(blog);
        await _applicationDbContext.SaveChangesAsync();

        blog.Id = result.Entity.Id;
        var blogResponse = _mapper.Map<BlogResponse>(blog);

        return blogResponse;
    }

    public async Task<GetBlogResponse> GetBlogAsync(Guid id)
    {
        var result = await _applicationDbContext.Blogs.FirstOrDefaultAsync(b => b.Id == id)
            ?? throw new NotFoundException("Böyle bir blog bulunamadı");

        var blogResponse = _mapper.Map<GetBlogResponse>(result);
        return blogResponse;
    }

    public async Task<List<GetBlogsResponse>> GetBlogsAsync()
    {
        var result = await _applicationDbContext.Blogs.ToListAsync();

        var blogResponse = _mapper.Map<List<GetBlogsResponse>>(result);
        return blogResponse;
    }

    private async Task<bool> IsExistAsync(Expression<Func<Blog, bool>> filter)
    {
        var result = await _applicationDbContext.Blogs.AnyAsync(filter);

        if (result)
            throw new ConflictException("Böyle bir başlık zaten var");

        return result;
    }
}
