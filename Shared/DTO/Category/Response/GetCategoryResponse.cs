using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTO.Category.Response
{
    public class GetCategoryResponse
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}