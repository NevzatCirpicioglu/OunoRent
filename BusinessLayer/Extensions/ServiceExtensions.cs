using System.Text;
using AutoMapper.Extensions.ExpressionMapping;
using BusinessLayer.CQRS.Category.Query;
using BusinessLayer.Mapper;
using BusinessLayer.Services;
using BusinessLayer.Validators;
using DataAccessLayer.Concrete;
using DataAccessLayer.Concrete.Repository;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.DTO.Authentication.Request;
using Shared.Interface;

namespace BusinessLayer.Extensions;

public static class ServiceExtensions
{
    /// <summary>
    /// Dependency Injection (DI) yapılandırmasını gerçekleştirir.
    /// Bu yöntem, gerekli servislerin DI konteynerine eklenmesini sağlar.
    /// </summary>
    /// <param name="services">Servis koleksiyonu.</param>
    private static void ConfigureDI(this IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ISliderRepository, SliderRepository>();
    }

    /// <summary>
    /// Veritabanı bağlamını (DbContext) yapılandırır.
    /// Bu yöntem, Npgsql kullanarak PostgreSQL veritabanı bağlantısını yapılandırır ve DI konteynerine ekler.
    /// </summary>
    /// <param name="services">Servis koleksiyonu.</param>
    /// <param name="configuration">Uygulama yapılandırma ayarları.</param>
    private static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }

    /// <summary>
    /// MediatR kütüphanesini yapılandırır.
    /// Bu yöntem, MediatR'yi belirli bir assembly'deki taleplerle (queries) kullanmak üzere DI konteynerine ekler.
    /// </summary>
    /// <param name="services">Servis koleksiyonu.</param>
    private static void ConfigureMediatr(this IServiceCollection services)
    {
        services.AddMediatR(typeof(GetCategoriesQuery).Assembly);
    }

    /// <summary>
    /// AutoMapper kütüphanesini yapılandırır.
    /// Bu yöntem, AutoMapper'ı belirli bir yapılandırma profili ve ifade eşleme desteği ile DI konteynerine ekler.
    /// </summary>
    /// <param name="services">Servis koleksiyonu.</param>
    private static void ConfigureAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MapperProfile>();
            cfg.AddExpressionMapping();
        });
    }

    /// <summary>
    /// JWT kimlik doğrulama ve yetkilendirme yapılandırmasını gerçekleştirir.
    /// Bu yöntem, JWT belirteçlerini doğrulamak için gerekli ayarları yapar ve kimlik doğrulama hizmetlerini DI konteynerine ekler.
    /// </summary>
    /// <param name="services">Servis koleksiyonu.</param>
    /// <param name="configuration">Uygulama yapılandırma ayarları.</param>
    private static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JWT");
        var key = jwtSettings["Key"];

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ClockSkew = TimeSpan.Zero,
            };
        });

        services.AddAuthorization();
    }

    public static void ConfigureFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
        //services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
    }

    public static void ConfigureCORS(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigin",
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
            );
        });
    }

    /// <summary>
    /// Tüm yapılandırma genişletme yöntemlerini çağırarak servisleri yapılandırır.
    /// Bu yöntem, veritabanı bağlamı, bağımlılık enjeksiyonu, MediatR, AutoMapper ve JWT kimlik doğrulama yapılandırmalarını gerçekleştirir.
    /// </summary>
    /// <param name="services">Servis koleksiyonu.</param>
    /// <param name="configuration">Uygulama yapılandırma ayarları.</param>
    public static void ConfigureAllExtensionMethods(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureDbContext(services, configuration);
        ConfigureDI(services);
        ConfigureMediatr(services);
        ConfigureAutoMapper(services);
        ConfigureJWT(services, configuration);
        ConfigureFluentValidation(services);
        ConfigureCORS(services);
    }
}