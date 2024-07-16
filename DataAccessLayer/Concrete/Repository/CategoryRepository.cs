using Shared.DTO.Category.Response;
using Shared.Interface;
using Microsoft.EntityFrameworkCore;
using EntityLayer.Entities;

namespace DataAccessLayer.Concrete.Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public CategoryRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    public async Task<List<GetCategoriesResponse>> GetCategories()
    {
        var categories = await _applicationDbContext.Categories
        .Select(x => new GetCategoriesResponse
        {
            CategoryId = x.Id,
            CategoryName = x.Name
        }).ToListAsync();

        return categories;
    }

    public async Task<GetCategoryResponse> GetCategory(int categoryId)
    {
        var category = await _applicationDbContext.Categories
        .Where(x => x.Id == categoryId)
        .Select(x => new GetCategoryResponse
        {
            CategoryId = x.Id,
            CategoryName = x.Name
        }).FirstOrDefaultAsync();

        return category;
    }

    public async Task<CategoryResponse> CreateCategory(string categoryName)
    {
        var category = new Category
        {
            Name = categoryName,
            CreatedDateTime = DateTime.UtcNow,
            ModifiedDateTime = DateTime.UtcNow,
            CreatedBy = "System",
            ModifiedBy = "System"
        };

        _applicationDbContext.Categories.Add(category);

        await _applicationDbContext.SaveChangesAsync();

        return new CategoryResponse
        {
            CategoryId = category.Id,
            CategoryName = category.Name,
            CreatedDateTime = category.CreatedDateTime,
            CreatedBy = category.CreatedBy,
            ModifiedDateTime = category.ModifiedDateTime,
            ModifiedBy = category.ModifiedBy
        };
    }
}