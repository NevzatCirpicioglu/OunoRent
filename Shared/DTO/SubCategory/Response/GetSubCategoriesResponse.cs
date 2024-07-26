using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTO.SubCategory.Response;

public class GetSubCategoriesResponse 
{
    public Guid CategoryId { get; set; }    

    public Guid SubCategoryId { get; set; } 

    public string Name { get; set; }    
}
