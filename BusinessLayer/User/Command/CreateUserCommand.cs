using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Shared.DTO.User.Request;
using Shared.DTO.User.Response;
using Shared.Interface;

namespace BusinessLayer.User.Command;

public sealed record CreateUserCommand(CreateUserRequest Request) : IRequest<UserResponse>
{
    internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserResponse>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            return await _userRepository.CreateUser(request.Request);
        }
    }
}
