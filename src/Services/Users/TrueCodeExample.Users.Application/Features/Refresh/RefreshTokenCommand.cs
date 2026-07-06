using Mediator;
using TrueCodeExample.Users.Application.DTO;

namespace TrueCodeExample.Users.Application.Features.Refresh;

public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<AuthResponse>;
