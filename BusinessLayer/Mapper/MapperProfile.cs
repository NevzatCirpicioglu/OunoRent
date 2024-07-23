using AutoMapper;
using Shared.DTO.Category.Response;
using EntityLayer.Entities;
using Shared.DTO.User.Response;
using Shared.DTO.Authentication.Response;

namespace BusinessLayer.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Category, GetCategoriesResponse>()
                 .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Name));

        CreateMap<Category, GetCategoryResponse>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Name));

        CreateMap<Category, CategoryResponse>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Name));

        CreateMap<User, UserResponse>();
        CreateMap<User, GetUserResponse>();
        CreateMap<User, GetUsersResponse>();
        CreateMap<User, UserDetailsResponse>();
    }
}
