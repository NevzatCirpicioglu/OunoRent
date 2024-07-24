using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTO.Slider.Response;

public class GetSliderResponse : GenericResponse
{
    public Guid SliderId { get; set; }

    public string Title { get; set; }

    public string  Url { get; set; }

    public string TargetUrl { get; set; }

    public int OrderNumber { get; set; }

    public Boolean IsActive { get; set; }
}
