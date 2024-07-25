using System.Linq.Expressions;
using AutoMapper;
using BusinessLayer.Middlewares;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.DTO.MenuItem.Request;
using Shared.DTO.MenuItem.Response;
using Shared.Interface;

namespace DataAccessLayer.Concrete.Repository;

public class MenuItemRepository : IMenuItemRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public MenuItemRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<MenuItemResponse> CreateMenuItemAsync(CreateMenuItemRequest createMenuItemRequest)
    {
        var existingMenuItem = await _applicationDbContext.MenuItems.FirstOrDefaultAsync(mi =>
            mi.OrderNumber == createMenuItemRequest.OrderNumber);

        if (existingMenuItem != null && existingMenuItem.IsActive)
            throw new ConflictException("Aynı sıra numarasına sahip iki adet slider mevcut olamaz." +
                "Lütfen her slider için benzersiz bir sıra numarası kullanın.");

        var menuItem = new MenuItem
        {
            Label = createMenuItemRequest.Label,
            TargetUrl = createMenuItemRequest.TargetUrl,
            OrderNumber = createMenuItemRequest.OrderNumber,
            OnlyToMembers = createMenuItemRequest.OnlyToMembers,
            IsActive = createMenuItemRequest.IsActive,
        };

        await _applicationDbContext.MenuItems.AddAsync(menuItem);
        await _applicationDbContext.SaveChangesAsync();

        var menuItemResponse = _mapper.Map<MenuItemResponse>(menuItem);
        return menuItemResponse;
    }

    public async Task<Guid> DeleteMenuItemAsync(Guid id)
    {
        var entity = await _applicationDbContext
            .MenuItems.FirstOrDefaultAsync(mi => mi.MenuItemId == id)
            ?? throw new NotFoundException("Menü öğesi bulunamadı.");

        _applicationDbContext.MenuItems.Remove(entity);
        await _applicationDbContext.SaveChangesAsync();

        return entity.MenuItemId;
    }

    public async Task<GetMenuItemResponse> GetMenuItemAsync(Guid id)
    {
        var entity = await _applicationDbContext.MenuItems.FirstOrDefaultAsync(mi => mi.MenuItemId == id)
            ?? throw new NotFoundException("Menü öğesi bulunamadı");

        var menuItemResponse = _mapper.Map<GetMenuItemResponse>(entity);
        return menuItemResponse;
    }

    public async Task<List<GetMenuItemsResponse>> GetMenuItemsAsync()
    {
        var entity = await _applicationDbContext.MenuItems.ToListAsync();
        var menuItemResponse = _mapper.Map<List<GetMenuItemsResponse>>(entity);

        return menuItemResponse;
    }

    public async Task<MenuItemResponse> UpdateMenuItemAsync(UpdateMenuItemRequest updateMenuItemRequest)
    {
        if (!await IsExistAsync(mi => mi.MenuItemId == updateMenuItemRequest.MenuItemId))
            throw new NotFoundException("Menü öğesi bulunamadı");

        var existingMenuItem = await _applicationDbContext.MenuItems.FirstOrDefaultAsync(mi =>
            mi.OrderNumber == updateMenuItemRequest.OrderNumber);

        if (existingMenuItem != null && existingMenuItem.IsActive
            && existingMenuItem.MenuItemId != updateMenuItemRequest.MenuItemId)
            throw new ConflictException("Aynı sıra numarasına sahip iki adet slider mevcut olamaz." +
                "Lütfen her slider için benzersiz bir sıra numarası kullanın.");

        var entity = new MenuItem
        {
            MenuItemId = updateMenuItemRequest.MenuItemId,
            Label = updateMenuItemRequest.Label,
            TargetUrl = updateMenuItemRequest.TargetUrl,
            OrderNumber = updateMenuItemRequest.OrderNumber,
            OnlyToMembers = updateMenuItemRequest.OnlyToMembers,
            IsActive = updateMenuItemRequest.IsActive,
        };

        _applicationDbContext.MenuItems.Update(entity);
        await _applicationDbContext.SaveChangesAsync();

        var menuItemResponse = _mapper.Map<MenuItemResponse>(entity);
        return menuItemResponse;
    }

    private async Task<bool> IsExistAsync(Expression<Func<MenuItem, bool>> filter)
    {
        return await _applicationDbContext.MenuItems.AnyAsync(filter);
    }
}
