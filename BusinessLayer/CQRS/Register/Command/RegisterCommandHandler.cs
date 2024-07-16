using MediatR;
using Shared.DTO.User.Response;
using Shared.Interface;

namespace BusinessLayer.CQRS.Register.Command;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, UserResponse>
{
    private readonly IAuthService _authService;

    public RegisterCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<UserResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterAsync(request.RegisterRequest);
        return result;
    }
}
