using AutoMapper.Extensions.ExpressionMapping;
using BusinessLayer.Category.Query;
using BusinessLayer.Mapper;
using DataAccessLayer.Concrete;
using DataAccessLayer.Concrete.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Interface;

namespace BusinessLayer.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureDI(this IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>();
    }

    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }

    public static void ConfigureMediatr(this IServiceCollection services)
    {
        services.AddMediatR(typeof(GetCategoriesQuery).Assembly);
    }

    public static void ConfigureAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MapperProfile>();
            cfg.AddExpressionMapping();
        });
    }

    public static void ConfigureAllExtensionMethods(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureDbContext(services, configuration);
        ConfigureDI(services);
        ConfigureMediatr(services);
        ConfigureAutoMapper(services);
    }
}