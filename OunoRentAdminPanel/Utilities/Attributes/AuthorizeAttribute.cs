using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OunoRentAdminPanel.Utilities.Attributes;

public class AuthAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthAttribute(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        //Sessionda depolanan token ve tokenın sona erme süresini getiriyor
        var token = context.HttpContext.Session.GetString("token");
        var expireTimeString = context.HttpContext.Session.GetString("expireTime");

        //Token yoksa unauthorized dönüyor
        if (token == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        //Eğer sona erme süresi sessionda varsa tokenın sona erip ermediğini
        //kontrol ediyor. Böylece kullanıcı her istek attığında apiye gitmeden
        //client tarafında tokenın geçerli olup olmadığını kontrol edebiliyoruz
        if (expireTimeString != null)
        {
            var expireTime = DateTime.Parse(expireTimeString);

            if (DateTime.Now < expireTime)
                return;
        }

        //Eğer token null değil ve geçerlilik süresi bitmişse apiye istek atıyoruz
        var client = _httpClientFactory.CreateClient("ounoRentApi");
        var content = new { Token = token };
        var response = await client.PostAsJsonAsync("auth/validate-token", content);

        //Validate token endpointinden dönen cevap başarılıysa response olarak tokenın
        //geçerlilik süresini döndürüyor. Dönen cevabı sessiona kaydederek bir sonraki
        //requestte kullanıyoruz
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadFromJsonAsync<JsonElement>();
            var responseString = jsonResponse.GetProperty("expireTime").GetString();

            if (DateTime.TryParse(responseString, out DateTime expTime))
            {
                context.HttpContext.Session.SetString("expireTime", expTime.ToString());
            }
        }

        client.Dispose();

        //Response eğer başarılı bir şekilde dönmezse tokenın süresi bitmiş demektir.
        //Bu durumda apiden gelen response headerında New-Token var mı diye kontrol
        //ediyoruz. New-Token refresh token sayesinde yeniden oluşturulan token.
        //Bu tokenı bir sonraki istekte kullanmak için sessiona kaydediyoruz
        if (response.Headers.Contains("New-Token"))
        {
            if (response.Headers.TryGetValues("New-Token", out var tokenValues))
            {
                var newToken = tokenValues.FirstOrDefault();
                if (newToken != null)
                {
                    context.HttpContext.Session.SetString("token", newToken);
                }
            }

            return;
        }

        //Yukarıdaki şartların hiçbirisi sağlanmamış ve apiden başarılı bir response
        //dönmemişse ve responsun status codeu unautherized ise ilk token ve refresh
        //token süreleri dolmuş demektir. Sessiondan token ve expireTime bilgisini
        //kaldırıyoruz. Böylece kullanıcı bir sonraki istekte otomatik olarak login
        //sayfasına yönlendirilicek
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            context.Result = new UnauthorizedResult();
            context.HttpContext.Session.Remove("token");
            context.HttpContext.Session.Remove("expireTime");
            return;
        }
    }
}
