using MediatR;
using Shared.DTO.Login.Request;
using Shared.DTO.Login.Response;
using Shared.Interface;

namespace BusinessLayer.CQRS.Login.Command;

public sealed record LoginCommand(LoginRequest LoginRequest) : IRequest<LoginResponse>;

internal class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var token = await _authService.LoginAsync(request.LoginRequest);
        return new LoginResponse { Token = token };
    }
}
