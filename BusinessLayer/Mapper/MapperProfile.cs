using AutoMapper;
using Shared.DTO.Category.Response;
using EntityLayer.Entities;
using Shared.DTO.User.Response;
using Shared.DTO.Authentication.Response;
using Shared.DTO.Slider.Response;
using Shared.DTO.SubCategory.Response;

namespace BusinessLayer.Mapper;

public class MapperProfile : Profile
{
        public MapperProfile()
        {
                CreateMap<Category, GetCategoriesResponse>();
                CreateMap<Category, GetCategoryResponse>();
                CreateMap<Category, CategoryResponse>();

                CreateMap<Slider, SliderResponse>()
                        .ForMember(dest => dest.SliderId, opt => opt.MapFrom(src => src.Id));

                CreateMap<Slider, GetSlidersResponse>()
                        .ForMember(dest => dest.SliderId, opt => opt.MapFrom(src => src.Id))
                        .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.ImgUrl));

                CreateMap<Slider, GetSliderResponse>()
                        .ForMember(dest => dest.SliderId, opt => opt.MapFrom(src => src.Id))
                        .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.ImgUrl));

                CreateMap<User, UserResponse>();
                CreateMap<User, GetUserResponse>();
                CreateMap<User, GetUsersResponse>();
                CreateMap<User, UserDetailsResponse>();
                CreateMap<SubCategory, SubCategoryResponse>();
                CreateMap<SubCategory, GetSubCategoriesResponse>();
                CreateMap<SubCategory, GetSubCategoryResponse>();
        }
}
