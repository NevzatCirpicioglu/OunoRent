using MediatR;
using Shared.DTO.MenuItem.Response;
using Shared.Interface;

namespace BusinessLayer.CQRS.MenuItem.Command;

public sealed record DeleteMenuItemommand(Guid MenuItemId) : IRequest<MenuItemResponse>;

public class DeleteMenuItemCommandHandler : IRequestHandler<DeleteMenuItemommand, MenuItemResponse>
{
    private readonly IMenuItemRepository _menuItemRepository;

    public DeleteMenuItemCommandHandler(IMenuItemRepository menuItemRepository)
    {
        _menuItemRepository = menuItemRepository;
    }

    public async Task<MenuItemResponse> Handle(DeleteMenuItemommand request, CancellationToken cancellationToken)
    {
        var menuItem = await _menuItemRepository.DeleteMenuItemAsync(request.MenuItemId);
        return menuItem;
    }
}
