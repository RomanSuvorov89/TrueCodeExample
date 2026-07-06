using Mediator;
using TrueCodeExample.Users.Application.Contracts;

namespace TrueCodeExample.Users.Application.Features.Register;

public sealed record RegisterUserCommand(string Name, string Password) : IRequest<AuthResponse>;
