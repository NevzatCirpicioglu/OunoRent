using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.DTO.Category.Response;

namespace Shared.Interface
{
    public interface ICategoryRepository
    {
        Task<List<GetCategoriesResponse>> GetCategories();

        Task<GetCategoryResponse> GetCategory(int categoryId);

        Task<CategoryResponse> CreateCategory(string categoryName);
    }
}