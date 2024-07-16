using MediatR;
using Shared.DTO.Category.Response;
using Shared.Interface;

namespace BusinessLayer.Category.Query;

public sealed record GetCategoryQuery(int CategoryId) : IRequest<GetCategoryResponse>
{
    internal class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, GetCategoryResponse>
    {
        ICategoryRepository _categoryRepository;

        public GetCategoryQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<GetCategoryResponse> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetCategory(request.CategoryId);

            return category;
        }
    }
}