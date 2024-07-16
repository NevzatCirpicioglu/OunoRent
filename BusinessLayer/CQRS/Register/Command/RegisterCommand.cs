using MediatR;
using Shared.DTO.Register.Request;
using Shared.DTO.User.Response;

namespace BusinessLayer.CQRS.Register.Command;

public sealed record RegisterCommand(RegisterRequest RegisterRequest) : IRequest<UserResponse>;
