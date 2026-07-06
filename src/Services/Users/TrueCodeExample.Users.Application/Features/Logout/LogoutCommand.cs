using Mediator;

namespace TrueCodeExample.Users.Application.Features.Logout;

public sealed record LogoutCommand(string Jti, DateTime ExpiresAtUtc) : IRequest;
