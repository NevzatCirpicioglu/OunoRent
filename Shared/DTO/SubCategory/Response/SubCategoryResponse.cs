using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTO.SubCategory.Response;

public class SubCategoryResponse : GenericResponse
{
    public Guid CategoryId { get; set; }

    public Guid SubCategoryId { get; set; }
}
