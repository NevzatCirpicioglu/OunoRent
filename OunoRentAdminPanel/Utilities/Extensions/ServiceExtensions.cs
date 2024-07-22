using Microsoft.Extensions.Options;
using OunoRentAdminPanel.Middlewares;
using OunoRentAdminPanel.Models.ApiModels;
using OunoRentAdminPanel.Services.Concrete;
using OunoRentAdminPanel.Services.Interface;
using OunoRentAdminPanel.Utilities.Attributes;

namespace OunoRentAdminPanel.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient("ounoRentApi", (serviceProvider, client) =>
        {
            var settings = serviceProvider
                .GetRequiredService<IOptions<ApiConfiguration>>().Value;

            //client.DefaultRequestHeaders.Add("User-Agent", settings.UserAgent);

            client.BaseAddress = new Uri("http://localhost:5244/api/");
        })
            .AddHttpMessageHandler<TokenHandler>();
    }

    public static void ConfigureDI(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<AuthAttribute>();
        services.AddTransient<TokenHandler>();
    }
}
