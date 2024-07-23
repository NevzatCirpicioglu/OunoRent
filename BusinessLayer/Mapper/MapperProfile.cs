using AutoMapper;
using Shared.DTO.Category.Response;
using EntityLayer.Entities;

namespace BusinessLayer.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Category, GetCategoriesResponse>()
                 .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Name));
    }
}
