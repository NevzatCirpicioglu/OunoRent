using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Middlewares;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
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
        await IsEsxitSlider(createSliderRequest.Title, createSliderRequest.OrderNumber);

        var slider = new Slider();

        slider.Title = createSliderRequest.Title;
        slider.ImgUrl = createSliderRequest.ImgUrl;
        slider.TargetUrl = createSliderRequest.TargetUrl;
        slider.OrderNumber = createSliderRequest.OrderNumber;
        slider.IsActive = createSliderRequest.IsActive;

        _applicationDbContext.Sliders.Add(slider);

        await _applicationDbContext.SaveChangesAsync();

        var sliderResponse = _mapper.Map<SliderResponse>(slider);

        return sliderResponse;

    }

    public async Task<Guid> DeleteSlider(Guid sliderId)
    {
        var slider = await _applicationDbContext.Sliders
        .FirstOrDefaultAsync(x => x.Id == sliderId);

        _applicationDbContext.Sliders.Remove(slider);

        await _applicationDbContext.SaveChangesAsync();

        return slider.Id;
    }

    public async Task<GetSliderResponse> GetSlider(Guid sliderId)
    {
        var slider = await _applicationDbContext.Sliders
        .FirstOrDefaultAsync(x => x.Id == sliderId)
        ?? throw new NotFoundException("Slider Bulunamadı");

        var getSliderResponse = _mapper.Map<GetSliderResponse>(slider);

        return getSliderResponse;
    }

    public async Task<List<GetSlidersResponse>> GetSliders()
    {
        var sliders = await _applicationDbContext.Sliders.ToListAsync();

        var getSlidersResponse = _mapper.Map<List<GetSlidersResponse>>(sliders);

        return getSlidersResponse;
    }

    public async Task<SliderResponse> UpdateSlider(UpdateSliderRequest updateSliderRequest)
    {
        var slider = await _applicationDbContext.Sliders.FirstOrDefaultAsync(x => x.Id == updateSliderRequest.Id)
        ?? throw new NotFoundException("Slider Bulunamadı");

        await IsEsxitSlider(updateSliderRequest.Title, updateSliderRequest.OrderNumber);

        slider.Title = updateSliderRequest.Title;
        slider.ImgUrl = updateSliderRequest.Url;
        slider.TargetUrl = updateSliderRequest.TargetUrl;
        slider.OrderNumber = updateSliderRequest.OrderNumber;
        slider.IsActive = updateSliderRequest.IsActive;

        await _applicationDbContext.SaveChangesAsync();

        var sliderResponse = _mapper.Map<SliderResponse>(slider);

        return sliderResponse;
    }

    private async Task IsEsxitSlider(string title, int orderNumber)
    {
        var isExistSlider = await _applicationDbContext.Sliders
        .AnyAsync(x => x.Title == title);

        var isExistOrderNumber = await _applicationDbContext.Sliders
        .AnyAsync(x => x.OrderNumber == orderNumber);

        if (isExistSlider)
        {
            throw new ConflictException("Slider already exists");
        }

        else if(isExistOrderNumber)
        {
            throw new ConflictException("Order number already exists");
        }
    }
}
