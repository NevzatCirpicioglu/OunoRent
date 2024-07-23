using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTO.Slider.Request;

public sealed record CreateSliderRequest(string Title, string ImgUrl, string TargetUrl, int Order, bool IsActive);
