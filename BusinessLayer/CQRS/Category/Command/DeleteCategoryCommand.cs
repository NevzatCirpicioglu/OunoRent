using MediatR;
using Shared.Interface;

namespace BusinessLayer.CQRS.Category.Command;

public sealed record DeleteCategoryCommand(Guid categoryId) : IRequest<Guid>;

internal class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Guid>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Guid> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        return await _categoryRepository.DeleteCategory(request.categoryId);
    }
}

