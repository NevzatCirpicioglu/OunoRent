using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Shared.DTO.Category.Request;
using Shared.DTO.Category.Response;
using Shared.Interface;

namespace BusinessLayer.CQRS.Category.Command;

public sealed record UpdateCategoryCommand(UpdateCategoryRequest category) : IRequest<CategoryResponse>
{
    internal class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryResponse>
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            return await _categoryRepository.UpdateCategory(request.category);
        }
    }
}
