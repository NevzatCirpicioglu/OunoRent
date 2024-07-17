using Shared.DTO.Category.Response;
using Shared.Interface;
using Microsoft.EntityFrameworkCore;
using EntityLayer.Entities;
using Shared.DTO.Category.Request;

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

    public async Task<GetCategoryResponse> GetCategory(Guid categoryId)
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

    public async Task<CategoryResponse> UpdateCategory(UpdateCategoryRequest request)
    {
        var category = await _applicationDbContext.Categories
        .Where(x => x.Id == request.CategoryId)
        .FirstOrDefaultAsync();

        category.Name = request.CategoryName;

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

    public async Task<Guid> DeleteCategory(Guid categoryId)
    {
        var category = await _applicationDbContext.Categories
        .Where(x => x.Id == categoryId)
        .FirstOrDefaultAsync()
        ?? throw new Exception("Category not found");

        _applicationDbContext.Categories.Remove(category);

        await _applicationDbContext.SaveChangesAsync();

       return category.Id;
    }
}