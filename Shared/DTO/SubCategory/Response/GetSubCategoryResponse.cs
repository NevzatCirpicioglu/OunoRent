using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTO.SubCategory.Response;

public class GetSubCategoryResponse 
{
    public Guid CategoryId { get; set; }    

    public Guid SubCategoryId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Icon { get; set; }

    public int OrderNumber { get; set; }

    public Boolean IsActive { get; set; }
}
