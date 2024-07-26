using Shared.DTO.Category.Response;
using Shared.Interface;
using Microsoft.EntityFrameworkCore;
using EntityLayer.Entities;
using Shared.DTO.Category.Request;
using BusinessLayer.Middlewares;
using AutoMapper;
using System.Security.Cryptography.X509Certificates;

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
            .Include(x => x.SubCategories)
            .AsNoTracking()
            .Select(x => new GetCategoriesResponse
            {
                CategoryId = x.CategoryId,
                Name = x.Name,
                SubCategories = x.SubCategories.Select(y => new GetCategoriesResponse.SubCategory
                {
                    SubCategoryId = y.SubCategoryId,
                    Name = y.Name,
                    Description = y.Description,
                    Icon = y.Icon,
                    OrderNumber = y.OrderNumber,
                    IsActive = y.IsActive
                }).ToList()
            })
            .ToListAsync();

        var categoryResponse = _mapper.Map<List<GetCategoriesResponse>>(categories);

        return categoryResponse;
    }

    #endregion

    #region GetCategory

    public async Task<GetCategoryResponse> GetCategory(Guid categoryId)
    {
        var category = await _applicationDbContext.Categories
                           .AsNoTracking()
                           .Where(x => x.CategoryId == categoryId)
                           .FirstOrDefaultAsync()
                       ?? throw new NotFoundException("Category not found");

        var categoryResponse = _mapper.Map<GetCategoryResponse>(category);

        return categoryResponse;
    }

    #endregion

    #region CreateCategory

    public async Task<CategoryResponse> CreateCategory(CreateCategoryRequest createCategoryRequest)
    {
        await IsExitsCategory(createCategoryRequest.Name, createCategoryRequest.OrderNumber);

        var category = new Category();

        category.Name = createCategoryRequest.Name;
        category.Description = createCategoryRequest.Description;
        category.Icon = createCategoryRequest.Icon;
        category.OrderNumber = createCategoryRequest.OrderNumber;
        category.ImageHorizontalUrl = createCategoryRequest.ImageHorizontalUrl;
        category.ImageSquareUrl = createCategoryRequest.ImageSquareUrl;
        category.IsActive = true;

        _applicationDbContext.Categories.Add(category);

        await _applicationDbContext.SaveChangesAsync();

        var categoryResponse = _mapper.Map<CategoryResponse>(category);

        return categoryResponse;
    }

    #endregion

    #region UpdateCategory

    public async Task<CategoryResponse> UpdateCategory(UpdateCategoryRequest updateCategoryRequest)
    {
        var category = await _applicationDbContext.Categories
                           .Where(x => x.CategoryId == updateCategoryRequest.CategoryId)
                           .FirstOrDefaultAsync()
                       ?? throw new NotFoundException("Category not found");

        category.Name = updateCategoryRequest.Name;
        category.Description = updateCategoryRequest.Description;
        category.Icon = updateCategoryRequest.Icon;
        category.OrderNumber = updateCategoryRequest.OrderNumber;
        category.ImageHorizontalUrl = updateCategoryRequest.ImageHorizontalUrl;
        category.ImageSquareUrl = updateCategoryRequest.ImageSquareUrl;
        category.IsActive = true;

        await _applicationDbContext.SaveChangesAsync();

        var categoryResponse = _mapper.Map<CategoryResponse>(category);
        return categoryResponse;
    }

    #endregion

    #region DeleteCategory

    public async Task<Guid> DeleteCategory(Guid categoryId)
    {
        var category = await _applicationDbContext.Categories
                           .Where(x => x.CategoryId == categoryId)
                           .FirstOrDefaultAsync()
                       ?? throw new NotFoundException("Category not found");

        await IsUsedCategory(categoryId);

        _applicationDbContext.Categories.Remove(category);

        await _applicationDbContext.SaveChangesAsync();

        return category.CategoryId;
    }

    #endregion

    #region IsExistCategory

    /// <summary>
    /// Verilen kategori adının mevcut olup olmadığını kontrol eder.
    /// </summary>
    /// <param name="categoryName">Kontrol edilecek kategori adı.</param>
    /// <returns>Mevcut kategori olup olmadığını kontrol eder ve kategori mevcutsa istisna fırlatır.</returns>
    /// <exception cref="KeyNotFoundException">Kategori zaten mevcutsa fırlatılır.</exception>
    private async Task IsExitsCategory(string categoryName, int orderNumber)
    {
        var isExist = await _applicationDbContext.Categories
            .AnyAsync(x => x.Name == categoryName);
        
        var isExistOrderNumber = await _applicationDbContext.Categories
            .AnyAsync(x => x.OrderNumber == orderNumber);
        
        if (isExist)
        {
            throw new ConflictException("Category already exist");
        }
        
        if (isExistOrderNumber)
        {
            throw new ConflictException("Order number already exists");
        }
    }

    #endregion

    #region IsUsedCategory

    private async Task IsUsedCategory(Guid categoryId)
    {
        var isUsed = await _applicationDbContext.SubCategories
            .AnyAsync(x => x.CategoryId == categoryId);

        if (isUsed)
        {
            throw new IsUsedException("Category is used");
        }
    }

    #endregion
}