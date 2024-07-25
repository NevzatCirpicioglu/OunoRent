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

    #region CreateSlider
    public async Task<SliderResponse> CreateSlider(CreateSliderRequest createSliderRequest)
    {
        await IsEsxitSlider(createSliderRequest.Title, createSliderRequest.OrderNumber);

        var slider = new Slider
        {
            Title = createSliderRequest.Title,
            MainImageUrl = createSliderRequest.MainImageUrl,
            MobileImageUrl = createSliderRequest.MobileImageUrl,
            TargetUrl = createSliderRequest.TargetUrl,
            OrderNumber = createSliderRequest.OrderNumber,
            ActiveFrom = createSliderRequest.ActiveFrom,
            ActiveTo = createSliderRequest.ActiveTo,
            Duration = createSliderRequest.Duration,
            IsActive = createSliderRequest.IsActive
        };

        _applicationDbContext.Sliders.Add(slider);

        await _applicationDbContext.SaveChangesAsync();

        var sliderResponse = _mapper.Map<SliderResponse>(slider);

        return sliderResponse;

    }

    #endregion

    #region DeleteSlider
    public async Task<Guid> DeleteSlider(Guid sliderId)
    {
        var slider = await _applicationDbContext.Sliders
        .FirstOrDefaultAsync(x => x.SliderId == sliderId)
        ?? throw new NotFoundException("Böyle bir öğe bulunamadı.");

        _applicationDbContext.Sliders.Remove(slider);

        await _applicationDbContext.SaveChangesAsync();

        return slider.SliderId;
    }

    #endregion

    #region GetSlider
    public async Task<GetSliderResponse> GetSlider(Guid sliderId)
    {
        var slider = await _applicationDbContext.Sliders
        .FirstOrDefaultAsync(x => x.SliderId == sliderId)
        ?? throw new NotFoundException("Slider Bulunamadı");

        var getSliderResponse = _mapper.Map<GetSliderResponse>(slider);

        return getSliderResponse;
    }

    #endregion

    #region GetSliders
    public async Task<List<GetSlidersResponse>> GetSliders()
    {
        var sliders = await _applicationDbContext.Sliders.ToListAsync();

        var getSlidersResponse = _mapper.Map<List<GetSlidersResponse>>(sliders);

        return getSlidersResponse;
    }

    #endregion

    #region UpdateSlider
    public async Task<SliderResponse> UpdateSlider(UpdateSliderRequest updateSliderRequest)
    {
        var slider = await _applicationDbContext.Sliders.FirstOrDefaultAsync(
            x => x.SliderId == updateSliderRequest.SliderId)
                ?? throw new NotFoundException("Slider Bulunamadı");

        await IsEsxitSlider(updateSliderRequest.Title, updateSliderRequest.OrderNumber);

        slider.Title = updateSliderRequest.Title;
        slider.MainImageUrl = updateSliderRequest.MainImageUrl;
        slider.MobileImageUrl = updateSliderRequest.MobileImageUrl;
        slider.TargetUrl = updateSliderRequest.TargetUrl;
        slider.OrderNumber = updateSliderRequest.OrderNumber;
        slider.Duration = updateSliderRequest.Duration;
        slider.ActiveFrom = updateSliderRequest.ActiveFrom;
        slider.ActiveTo = updateSliderRequest.ActiveTo;
        slider.IsActive = updateSliderRequest.IsActive;

        await _applicationDbContext.SaveChangesAsync();

        var sliderResponse = _mapper.Map<SliderResponse>(slider);

        return sliderResponse;
    }

    #endregion

    #region IsEsxitSlider
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

        else if (isExistOrderNumber)
        {
            throw new ConflictException("Order number already exists");
        }
    }
    #endregion
}



