using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Shared.DTO.User.Request;
using Shared.DTO.User.Response;
using Shared.Interface;

namespace BusinessLayer.User.Query;

public sealed record GetUserQuery(Guid UserId) : IRequest<GetUserResponse>
{
    internal class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserResponse>
    {
        private readonly IUserRepository _userRepository;

        public GetUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetUserResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUserById(request.UserId);
        }
    }
}

