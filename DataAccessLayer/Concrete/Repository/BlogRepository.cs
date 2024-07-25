using System.Linq.Expressions;
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
        var isExistSubCategory = await _applicationDbContext.SubCategories
        .AnyAsync(x => x.SubCategoryId == createBlogRequest.SubCategoryId);
        if (!isExistSubCategory)
            throw new NotFoundException("Alt kategori bulunamadı");

        var sanitizer = new HtmlSanitizer();
        var blog = new Blog
        {
            LargeImageUrl = createBlogRequest.LargeImageUrl,
            OrderNumber = createBlogRequest.OrderNumber,
            Slug = createBlogRequest.Slug,
            SmallImageUrl = createBlogRequest.SmallImageUrl,
            IsActive = true,
            Date = DateTime.UtcNow,
            SubCategoryId = createBlogRequest.SubCategoryId,
            Body = sanitizer.Sanitize(createBlogRequest.Body),
            Title = sanitizer.Sanitize(createBlogRequest.Title),
            Tags = sanitizer.Sanitize(createBlogRequest.Tags)
        };

        var result = await _applicationDbContext.Blogs.AddAsync(blog);

        await _applicationDbContext.SaveChangesAsync();

        var blogResponse = _mapper.Map<BlogResponse>(blog);

        return blogResponse;
    }
    #endregion

    #region GetBlog
    public async Task<GetBlogResponse> GetBlogAsync(Guid blogId)
    {
        var result = await _applicationDbContext.Blogs
        .Include(x => x.SubCategory)
        .FirstOrDefaultAsync(b => b.BlogId == blogId)
            ?? throw new NotFoundException("Blog bulunamadı");

        var blogResponse = _mapper.Map<GetBlogResponse>(result);

        return blogResponse;
    }
    #endregion

    #region GetBlogs
    public async Task<List<GetBlogsResponse>> GetBlogsAsync()
    {
        var blogList = await _applicationDbContext.Blogs
        .Include(x => x.SubCategory)
        .ToListAsync();

        var blogResponse = _mapper.Map<List<GetBlogsResponse>>(blogList);

        return blogResponse;
    }
    #endregion

    #region IsExist
    private async Task<bool> IsExistAsync(Expression<Func<Blog, bool>> filter)
    {
        var result = await _applicationDbContext.Blogs.AnyAsync(filter);

        return result;
    }
    #endregion

    #region DeleteBlog
    public async Task<Guid> DeleteBlog(Guid blogId)
    {
        var blog = await _applicationDbContext.Blogs
            .Where(x => x.BlogId == blogId)
            .FirstOrDefaultAsync()
        ?? throw new NotFoundException("Blog bulunamadı");

        _applicationDbContext.Blogs.Remove(blog);

        await _applicationDbContext.SaveChangesAsync();

        return blog.BlogId;
    }

    #endregion

    #region UpdateBlog
    public async Task<BlogResponse> UpdateBlog(UpdateBlogRequest updateBlogRequest)
    {
        var blog = await _applicationDbContext.Blogs
            .Where(x => x.BlogId == updateBlogRequest.BlogId)
            .FirstOrDefaultAsync()
        ?? throw new NotFoundException("Blog bulunamadı");

        var isExistSubCategory = await _applicationDbContext.SubCategories
         .AnyAsync(x => x.SubCategoryId == updateBlogRequest.SubCategoryId);
        if (!isExistSubCategory)
            throw new NotFoundException("Alt kategori bulunamadı");

        blog.Title = updateBlogRequest.Title;
        blog.Tags = updateBlogRequest.Tags;
        blog.Slug = updateBlogRequest.Slug;
        blog.OrderNumber = updateBlogRequest.OrderNumber;
        blog.Date = updateBlogRequest.Date;
        blog.SubCategoryId = updateBlogRequest.SubCategoryId;
        blog.LargeImageUrl = updateBlogRequest.LargeImgUrl;
        blog.SmallImageUrl = updateBlogRequest.SmallImgUrl;
        blog.IsActive = updateBlogRequest.IsActive;

        await _applicationDbContext.SaveChangesAsync();

        var blogResponse = _mapper.Map<BlogResponse>(blog);

        return blogResponse;
    }

    #endregion
}
