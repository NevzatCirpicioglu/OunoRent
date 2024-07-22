namespace Shared.DTO.Category.Request;

public sealed record UpdateCategoryRequest(
    Guid CategoryId,
    string CategoryName
);

