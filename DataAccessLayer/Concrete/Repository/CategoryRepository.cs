using Shared.DTO.Category.Response;
using Shared.Interface;
using Microsoft.EntityFrameworkCore;
using EntityLayer.Entities;
using Shared.DTO.Category.Request;
using BusinessLayer.Middlewares;
using AutoMapper;
using System.Collections.Generic;

namespace DataAccessLayer.Concrete.Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public CategoryRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    #region GetCategories
    public async Task<List<GetCategoriesResponse>> GetCategories()
    {
        var categories = await _applicationDbContext.Categories
        .AsNoTracking()
        .ToListAsync();

        var categoriesResponse = _mapper.Map<List<GetCategoriesResponse>>(categories);
        return categoriesResponse;
    }
    #endregion

    #region GetCategory
    public async Task<GetCategoryResponse> GetCategory(Guid categoryId)
    {
        var category = await _applicationDbContext.Categories
        .AsNoTracking()
        .Where(x => x.Id == categoryId)
        .FirstOrDefaultAsync()
        ?? throw new NotFoundException("Category not found");

        var categoryResponse = _mapper.Map<GetCategoryResponse>(category);
        return categoryResponse;
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

        var categoryResponse = _mapper.Map<CategoryResponse>(category);
        return categoryResponse;
    }
    #endregion

    #region UpdateCategory
    public async Task<CategoryResponse> UpdateCategory(UpdateCategoryRequest request)
    {
        var category = await _applicationDbContext.Categories
        .Where(x => x.Id == request.CategoryId)
        .FirstOrDefaultAsync()
        ?? throw new NotFoundException("Category not found");

        category.Name = request.CategoryName;

        await _applicationDbContext.SaveChangesAsync();

        var categoryResponse = _mapper.Map<CategoryResponse>(category);
        return categoryResponse;
    }
    #endregion

    #region DeleteCategory
    public async Task<Guid> DeleteCategory(Guid categoryId)
    {
        var category = await _applicationDbContext.Categories
        .Where(x => x.Id == categoryId)
        .FirstOrDefaultAsync()
        ?? throw new NotFoundException("Category not found");

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
            throw new ConflictException("Category already exist");
        }
    }
    #endregion

}