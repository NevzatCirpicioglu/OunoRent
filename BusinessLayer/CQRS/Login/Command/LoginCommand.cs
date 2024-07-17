using MediatR;
using Shared.DTO.Login.Request;
using Shared.DTO.Login.Response;

namespace BusinessLayer.CQRS.Login.Command;

public sealed record LoginCommand(LoginRequest LoginRequest) : IRequest<LoginResponse>;
