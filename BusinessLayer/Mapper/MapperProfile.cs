using AutoMapper;
using EntityLayer;
using Shared.DTO.Category.Response;
using EntityLayer.Entities;
using Shared.DTO;
using Shared.DTO.User.Response;
using Shared.DTO.Authentication.Response;
using Shared.DTO.Slider.Response;
using Shared.DTO.Blog.Response;
using Shared.DTO.Blog.Request;
using Shared.DTO.SubCategory.Response;
using Shared.DTO.MenuItem.Response;

namespace BusinessLayer.Mapper;

public class MapperProfile : Profile
{
	public MapperProfile()
	{
		
		#region Category

		CreateMap<Category, GetCategoriesResponse>();
		CreateMap<Category, GetCategoryResponse>();
		CreateMap<Category, CategoryResponse>();

		#endregion

		#region Slider

		CreateMap<Slider, SliderResponse>();
		CreateMap<Slider, GetSlidersResponse>();
		CreateMap<Slider, GetSliderResponse>();

		#endregion

		#region User

		CreateMap<User, UserResponse>();
		CreateMap<User, GetUserResponse>();
		CreateMap<User, GetUsersResponse>();
		CreateMap<User, UserDetailsResponse>();

		#endregion

		#region SubCategory

		CreateMap<SubCategory, SubCategoryResponse>();
		CreateMap<SubCategory, GetSubCategoriesResponse>();
		CreateMap<SubCategory, GetSubCategoryResponse>();

		#endregion
		
		#region Blog

		CreateMap<Blog, BlogResponse>();
		CreateMap<Blog, GetBlogResponse>()
			.ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.SubCategory.Name));
		CreateMap<Blog, GetBlogsResponse>();

		#endregion

		#region MenuItem

		CreateMap<MenuItem, MenuItemResponse>();
		CreateMap<MenuItem, GetMenuItemResponse>();
		CreateMap<MenuItem, GetMenuItemsResponse>();

		#endregion
	}
}
