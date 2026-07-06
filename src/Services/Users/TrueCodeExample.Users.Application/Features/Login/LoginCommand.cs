using Mediator;
using TrueCodeExample.Users.Application.Contracts;

namespace TrueCodeExample.Users.Application.Features.Login;

public sealed record LoginCommand(string Name, string Password) : IRequest<AuthResponse>;
