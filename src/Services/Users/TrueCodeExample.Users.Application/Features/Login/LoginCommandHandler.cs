using Mediator;
using TrueCodeExample.Users.Application.Abstractions;
using TrueCodeExample.Users.Application.Contracts;
using TrueCodeExample.Users.Domain.Exceptions;

namespace TrueCodeExample.Users.Application.Features.Login;

public sealed class LoginCommandHandler(
    IUserRepository users,
    IPasswordHasher passwordHasher,
    ITokenService tokenService)
    : IRequestHandler<LoginCommand, AuthResponse>
{
    public async ValueTask<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await users.GetByNameAsync(request.Name, cancellationToken);

        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new InvalidCredentialsException();
        }

        var token = tokenService.GenerateToken(user);
        return new AuthResponse(user.Id, token.AccessToken, token.ExpiresAtUtc);
    }
}
