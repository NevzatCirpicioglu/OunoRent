using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Shared.DTO.User.Response;
using Shared.Interface;

namespace BusinessLayer.User.Query;

public sealed record GetUsersQuery : IRequest<List<GetUsersResponse>>
{
    internal class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<GetUsersResponse>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<GetUsersResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUsers();
        }
    }
}
