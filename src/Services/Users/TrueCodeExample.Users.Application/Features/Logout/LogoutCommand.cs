using Mediator;

namespace TrueCodeExample.Users.Application.Features.Logout;

public sealed record LogoutCommand(Guid UserId) : IRequest;
