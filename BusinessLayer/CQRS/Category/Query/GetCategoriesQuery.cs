using MediatR;
using Shared.DTO.Category.Response;
using Shared.Interface;

namespace BusinessLayer.Category.Query;

public sealed record GetCategoriesQuery() : IRequest<List<GetCategoriesResponse>>;

internal class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<GetCategoriesResponse>>
{
    ICategoryRepository _categoryRepository;

    public GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<GetCategoriesResponse>>
     Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetCategories();

        return categories;
    }
}
