using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTO;

public class GenericResponse
{
    public DateTime? CreatedDateTime { get; set; }
    public DateTime? ModifiedDateTime { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }
}
