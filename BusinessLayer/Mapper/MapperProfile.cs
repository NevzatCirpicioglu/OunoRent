using AutoMapper;
using Shared.DTO.Category.Response;
using EntityLayer.Entities;
using Shared.DTO.User.Response;
using Shared.DTO.Authentication.Response;
using Shared.DTO.Slider.Response;
using Shared.DTO.Blog.Response;
using Shared.DTO.Blog.Request;

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

		CreateMap<Blog, BlogResponse>()
				.ForMember(dest => dest.BlogId, opt => opt.MapFrom(src => src.Id));
		CreateMap<Blog, GetBlogResponse>()
				.ForMember(dest => dest.BlogId, opt => opt.MapFrom(src => src.Id));
		CreateMap<Blog, GetBlogsResponse>()
				.ForMember(dest => dest.BlogId, opt => opt.MapFrom(src => src.Id));
	}
}