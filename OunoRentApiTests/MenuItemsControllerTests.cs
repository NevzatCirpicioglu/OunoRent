using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Shared.DTO.MenuItem.Request;
using Shared.DTO.MenuItem.Response;

namespace OunoRentApiTests;

public class MenuItemsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{

    private readonly HttpClient _client;

    public MenuItemsControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    #region GetMenuItems
    [Fact]
    public async Task GetMenuItems_ReturnsSuccess()
    {
        //Act
        var response = await _client.GetAsync("/api/MenuItem");
        var content = await response.Content.ReadAsStringAsync();

        //Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(content);
    }
    #endregion

    #region CreateMenuItem
    [Fact]
    public async Task CreateMenuItem_ReturnsSuccess()
    {
        var body = new CreateMenuItemRequest(
            Label: "string",
            TargetUrl: "string",
            OrderNumber: 1,
            OnlyToMembers: true,
            IsActive: true
        );

        var response = await _client.PostAsJsonAsync("/api/menuitem/", body);
        var content = await response.Content.ReadFromJsonAsync<MenuItemResponse>();

        response.EnsureSuccessStatusCode();
        Assert.NotNull(content);
        Assert.NotEqual(content.MenuItemId, Guid.Empty);
    }

    [Fact]
    public async Task CreateMenuItem_ReturnBadRequest()
    {
        var body = new CreateMenuItemRequest(
            Label: null,
            TargetUrl: "string",
            OrderNumber: 1,
            OnlyToMembers: true,
            IsActive: true
        );

        var response = await _client.PostAsJsonAsync("/api/menuitem/", body);
        var content = await response.Content.ReadFromJsonAsync<MenuItemResponse>();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(content.MenuItemId, Guid.Empty);
    }
    #endregion

    #region GetMenuItem
    [Fact]
    public async Task GetMenuItem_ReturnsSuccess()
    {

        //<-------------------------------------------------------->
        //Bir id çekmek için tüm verileri çektim
        var getAllResponse = await _client.GetAsync("/api/MenuItem");
        getAllResponse.EnsureSuccessStatusCode();

        var getAllContent = await getAllResponse.Content.ReadFromJsonAsync<List<GetMenuItemsResponse>>();
        var menuItemId = getAllContent.FirstOrDefault().MenuItemId;
        //<-------------------------------------------------------->

        //Act
        var getResponse = await _client.GetAsync($"/api/MenuItem/{menuItemId}");
        getResponse.EnsureSuccessStatusCode();

        var getContent = await getResponse.Content.ReadFromJsonAsync<GetMenuItemResponse>();

        //Assert
        Assert.NotNull(getAllContent);
        Assert.NotNull(getContent);
        Assert.Equal(getContent.MenuItemId, menuItemId);
    }

    [Fact]
    public async Task GetMenuItem_ReturnsNotFound()
    {
        //Act
        var getResponse = await _client.GetAsync($"/api/MenuItem/49e7697d-7d11-404d-af72-beed870e301c");
        var getContent = await getResponse.Content.ReadFromJsonAsync<GetMenuItemResponse>();

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        Assert.Equal(getContent, new GetMenuItemResponse());
    }
    #endregion

    #region UpdateMenuItem
    [Fact]
    public async Task UpdateMenuItem_ReturnsSuccess()
    {
        //<-------------------------------------------------------->
        //Bir id çekmek için tüm verileri çektim
        var getAllResponse = await _client.GetAsync("/api/MenuItem");
        getAllResponse.EnsureSuccessStatusCode();

        var getAllContent = await getAllResponse.Content.ReadFromJsonAsync<List<GetMenuItemsResponse>>();
        var menuItem = getAllContent.FirstOrDefault();
        //<-------------------------------------------------------->

        //Act
        var updatedMenuItem = new UpdateMenuItemRequest(
            MenuItemId: menuItem.MenuItemId,
            Label: "Merhaba",
            TargetUrl: menuItem.TargetUrl,
            OrderNumber: menuItem.OrderNumber,
            OnlyToMembers: menuItem.OnlyToMembers,
            IsActive: menuItem.IsActive
        );

        var updateResponse = await _client.PutAsJsonAsync($"/api/menuitem/{menuItem.MenuItemId}", updatedMenuItem);
        updateResponse.EnsureSuccessStatusCode();

        var updateContent = await updateResponse.Content.ReadFromJsonAsync<MenuItemResponse>();

        //<-------------------------------------------------------->
        //Aynı datanın update olmuş halini çektim
        var getResponse = await _client.GetAsync($"/api/MenuItem/{menuItem.MenuItemId}");
        getResponse.EnsureSuccessStatusCode();

        var getContent = await getResponse.Content.ReadFromJsonAsync<GetMenuItemResponse>();
        //<-------------------------------------------------------->

        //Assert
        Assert.NotNull(updateContent);
        Assert.NotNull(getAllContent);
        Assert.Equal(updateContent.MenuItemId, menuItem.MenuItemId);
        Assert.Equal(getContent.Label, updatedMenuItem.Label);
        Assert.NotEqual(getContent.Label, menuItem.Label);
    }

    [Fact]
    public async Task UpdateMenuItem_ReturnsBadRequest()
    {
        var updatedMenuItem = new UpdateMenuItemRequest(
            MenuItemId: Guid.Empty,
            Label: "",
            TargetUrl: "",
            OrderNumber: 1,
            OnlyToMembers: true,
            IsActive: false
        );

        var response = await _client.PutAsJsonAsync($"/api/MenuItem/49e7697d-7d11-404d-af72-beed870e301c", updatedMenuItem);
        var content = await response.Content.ReadFromJsonAsync<MenuItemResponse>();

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal(new MenuItemResponse(), content);
    }
    #endregion

    #region DeleteMenuItem
    [Fact]
    public async Task DeleteMenuItem_ReturnsSuccess()
    {
        //<-------------------------------------------------------->
        //Bir id çekmek için tüm verileri çektim
        var getAllResponse = await _client.GetAsync("/api/MenuItem");
        getAllResponse.EnsureSuccessStatusCode();

        var getAllContent = await getAllResponse.Content.ReadFromJsonAsync<List<GetMenuItemsResponse>>();

        var menuItemId = getAllContent.FirstOrDefault().MenuItemId;
        //<-------------------------------------------------------->

        //Act
        var deleteResponse = await _client.DeleteAsync($"/api/menuitem/{menuItemId}");
        deleteResponse.EnsureSuccessStatusCode();

        var deleteContent = await deleteResponse.Content.ReadFromJsonAsync<MenuItemResponse>();

        //<-------------------------------------------------------->
        //Sildiğim idyi kontrol için tekrar çektim
        var getResponse = await _client.GetAsync($"/api/MenuItem/{menuItemId}");
        var getContent = await getResponse.Content.ReadFromJsonAsync<GetMenuItemResponse>();
        //<-------------------------------------------------------->

        //Assert
        Assert.NotNull(getAllContent);
        Assert.NotNull(deleteContent);
        Assert.NotNull(getAllContent);
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteMenuItems_ReturnsNotFound()
    {
        var deleteResponse = await _client.DeleteAsync($"/api/menuitem/49e7697d-7d11-404d-af72-beed870e301c");
        var deleteContent = await deleteResponse.Content.ReadFromJsonAsync<MenuItemResponse>();

        Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
        Assert.Equal(new MenuItemResponse(), deleteContent);
    }
    #endregion
}