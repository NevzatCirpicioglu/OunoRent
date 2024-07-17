using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTO.Category.Request;

public sealed record UpdateCategoryRequest(
    Guid CategoryId,
    string CategoryName
);

