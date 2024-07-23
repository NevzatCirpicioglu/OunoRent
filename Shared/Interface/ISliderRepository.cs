using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.DTO.Slider.Request;
using Shared.DTO.Slider.Response;

namespace Shared.Interface;

public interface ISliderRepository
{
    Task<SliderResponse> CreateSlider(CreateSliderRequest createSliderRequest);
}
