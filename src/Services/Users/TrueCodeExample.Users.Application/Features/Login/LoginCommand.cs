using Mediator;
using TrueCodeExample.Users.Application.DTO;

namespace TrueCodeExample.Users.Application.Features.Login;

public sealed record LoginCommand(string Name, string Password) : IRequest<AuthResponse>;
