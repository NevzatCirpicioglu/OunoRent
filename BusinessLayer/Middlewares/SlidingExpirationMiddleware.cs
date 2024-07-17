using Microsoft.AspNetCore.Http;
using Shared.Interface;

namespace BusinessLayer.Middlewares;

public class SlidingExpirationMiddleware
{
    private readonly RequestDelegate _next;

    public SlidingExpirationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITokenService _tokenService)
    {
        if (context.Request.Headers.ContainsKey("Authorization"))
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var newToken = await _tokenService.RefreshTokenAsync(token);
            if (newToken != null)
            {
                context.Response.Headers.Append("New-Token", newToken);
                context.Request.Headers["Authorization"] = "Bearer " + newToken;
            }

        }

        await _next(context);
    }
}
