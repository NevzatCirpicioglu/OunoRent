using System.Net.Http.Headers;

namespace OunoRentAdminPanel.Middlewares;

public class TokenHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        //Sessionda token varsa isteğin headerına tokenı ekliyoruz. Eğer token yoksa
        //isteği authorization headersız gönderiyoruz. Apide kullanılan bir middleware
        //authorization headerının olmadığını tespit edip. Unauthorized result döndürüyor
        //MVC unauhtorize ile dönen response sonrası kullanıcıyı login sayfasına yönlendiriyor.
        var token = _httpContextAccessor.HttpContext.Session.GetString("token");
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
