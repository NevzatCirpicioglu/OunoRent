using System.Net;

namespace OunoRentAdminPanel.Utilities.Extensions;

public static class ApplicationExtensions
{
    public static void UseStatusCodeRedirect(this IApplicationBuilder app)
    {
        app.UseStatusCodePages(context =>
        {
            var response = context.HttpContext.Response;

            if (response.StatusCode == (int)HttpStatusCode.Unauthorized ||
                response.StatusCode == (int)HttpStatusCode.Forbidden)
            {
                response.Redirect("/auth/login");
            }

            return Task.CompletedTask;
        });
    }
}
