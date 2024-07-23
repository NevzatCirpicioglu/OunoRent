using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EntityLayer.Entities;
using Shared.DTO.Slider.Request;
using Shared.DTO.Slider.Response;
using Shared.Interface;

namespace DataAccessLayer.Concrete.Repository;

public class SliderRepository : ISliderRepository
{

    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public SliderRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<SliderResponse> CreateSlider(CreateSliderRequest createSliderRequest)
    {
        var slider = new Slider();

        slider.Title = createSliderRequest.Title;
        slider.ImgUrl = createSliderRequest.ImgUrl;
        slider.TargetUrl = createSliderRequest.TargetUrl;
        slider.Order = createSliderRequest.Order;
        slider.IsActive = createSliderRequest.IsActive;

        _applicationDbContext.Sliders.Add(slider);

        await _applicationDbContext.SaveChangesAsync();

        var sliderResponse = _mapper.Map<SliderResponse>(slider);

        return sliderResponse;

    }
}
