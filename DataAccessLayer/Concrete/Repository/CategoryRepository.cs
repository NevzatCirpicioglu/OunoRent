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

    #region GetCategories
    public async Task<List<GetCategoriesResponse>> GetCategories()
    {
        var categories = await _applicationDbContext.Categories
        .AsNoTracking()
        .Select(x => new GetCategoriesResponse
        {
            CategoryId = x.Id,
            CategoryName = x.Name,
            CreatedDateTime = x.CreatedDateTime,
            CreatedBy = x.CreatedBy,
            ModifiedDateTime = x.ModifiedDateTime,
            ModifiedBy = x.ModifiedBy
        }).ToListAsync();

        return categories;
    }
    #endregion

    #region GetCategory
    public async Task<GetCategoryResponse> GetCategory(Guid categoryId)
    {
        var category = await _applicationDbContext.Categories
        .AsNoTracking()
        .Where(x => x.Id == categoryId)
        .Select(x => new GetCategoryResponse
        {
            CategoryId = x.Id,
            CategoryName = x.Name,
            CreatedDateTime = x.CreatedDateTime,
            CreatedBy = x.CreatedBy,
            ModifiedDateTime = x.ModifiedDateTime,
            ModifiedBy = x.ModifiedBy
        }).FirstOrDefaultAsync()
        ?? throw new KeyNotFoundException("Category not found");

        return category;
    }
    #endregion

    #region CreateCategory
    public async Task<CategoryResponse> CreateCategory(string categoryName)
    {

        await IsEsxitCategory(categoryName);

        var category = new Category();

        category.Name = categoryName;

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
    #endregion

    #region UpdateCategory
    public async Task<CategoryResponse> UpdateCategory(UpdateCategoryRequest request)
    {
        var category = await _applicationDbContext.Categories
        .Where(x => x.Id == request.CategoryId)
        .FirstOrDefaultAsync()
        ?? throw new KeyNotFoundException("Category not found");

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
    #endregion

    #region DeleteCategory
    public async Task<Guid> DeleteCategory(Guid categoryId)
    {
        var category = await _applicationDbContext.Categories
        .Where(x => x.Id == categoryId)
        .FirstOrDefaultAsync()
        ?? throw new KeyNotFoundException("Category not found");

        _applicationDbContext.Categories.Remove(category);

        await _applicationDbContext.SaveChangesAsync();

        return category.Id;
    }
    #endregion


    #region IsEsisCategory
    /// <summary>
    /// Verilen kategori adının mevcut olup olmadığını kontrol eder.
    /// </summary>
    /// <param name="categoryName">Kontrol edilecek kategori adı.</param>
    /// <returns>Mevcut kategori olup olmadığını kontrol eder ve kategori mevcutsa istisna fırlatır.</returns>
    /// <exception cref="KeyNotFoundException">Kategori zaten mevcutsa fırlatılır.</exception>
    private async Task IsEsxitCategory(string categoryName)
    {
        var isExist = await _applicationDbContext.Categories
        .AnyAsync(x => x.Name == categoryName);

        if (isExist)
        {
            throw new KeyNotFoundException("Category already exist");
        }
    }
    #endregion

}