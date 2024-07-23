using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTO.Slider.Request;

public sealed record UpdateSliderRequest(Guid Id, string Title, string Url, string TargetUrl, int OrderNumber, Boolean IsActive);

