using MediatR;
using Shared.DTO.User.Response;
using Shared.Interface;

namespace BusinessLayer.User.Command;

public sealed record DeleteUserCommand(Guid UserId) : IRequest<UserResponse>;

internal class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, UserResponse>
{
    IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.DeleteUser(request.UserId);

        return user;
    }
}

