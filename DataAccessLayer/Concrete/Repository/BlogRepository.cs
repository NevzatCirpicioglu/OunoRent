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

    #region CreateBlog
    public async Task<BlogResponse> CreateBlogAsync(CreateBlogRequest createBlogRequest)
    {
        var sanitizer = new HtmlSanitizer();
        var blog = new Blog
        {
            LargeImageUrl = createBlogRequest.LargeImageUrl,
            OrderNumber = createBlogRequest.OrderNumber,
            Slug = createBlogRequest.Slug,
            SmallImageUrl = createBlogRequest.SmallImageUrl,
            Tags = createBlogRequest.Tags,
            IsActive = true,
            Date = DateTime.UtcNow
        };

        blog.Body = sanitizer.Sanitize(createBlogRequest.Body);
        blog.Title = sanitizer.Sanitize(createBlogRequest.Title);
        blog.Tags = sanitizer.Sanitize(createBlogRequest.Tags);

        var result = await _applicationDbContext.Blogs.AddAsync(blog);

        await _applicationDbContext.SaveChangesAsync();

        blog.Id = result.Entity.Id;

        var blogResponse = _mapper.Map<BlogResponse>(blog);

        return blogResponse;
    }
    #endregion

    #region GetBlog
    public async Task<GetBlogResponse> GetBlogAsync(Guid id)
    {
        var result = await _applicationDbContext.Blogs.FirstOrDefaultAsync(b => b.Id == id)
            ?? throw new NotFoundException("Böyle bir blog bulunamadı");

        var blogResponse = _mapper.Map<GetBlogResponse>(result);
        return blogResponse;
    }
    #endregion

    #region GetBlogs
    public async Task<List<GetBlogsResponse>> GetBlogsAsync()
    {
        var result = await _applicationDbContext.Blogs.ToListAsync();

        var blogResponse = _mapper.Map<List<GetBlogsResponse>>(result);
        return blogResponse;
    }
    #endregion

    #region IsExist
    private async Task<bool> IsExistAsync(Expression<Func<Blog, bool>> filter)
    {
        var result = await _applicationDbContext.Blogs.AnyAsync(filter);

        if (result)
            throw new ConflictException("Böyle bir başlık zaten var");

        return result;
    }
    #endregion

    #region DeleteBlog
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

    #endregion

    #region UpdateBlog
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

    #endregion
}
