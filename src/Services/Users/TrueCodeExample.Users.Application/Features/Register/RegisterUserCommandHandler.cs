using Mediator;
using TrueCodeExample.Users.Application.Abstractions;
using TrueCodeExample.Users.Application.Contracts;
using TrueCodeExample.Users.Domain.Entities;
using TrueCodeExample.Users.Domain.Exceptions;

namespace TrueCodeExample.Users.Application.Features.Register;

public sealed class RegisterUserCommandHandler(
    IUserRepository users,
    IPasswordHasher passwordHasher,
    ITokenService tokenService)
    : IRequestHandler<RegisterUserCommand, AuthResponse>
{
    public async ValueTask<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await users.ExistsByNameAsync(request.Name, cancellationToken))
        {
            throw new UserAlreadyExistsException(request.Name);
        }

        var user = new User(request.Name, passwordHasher.Hash(request.Password));

        await users.AddAsync(user, cancellationToken);
        await users.SaveChangesAsync(cancellationToken);

        var token = tokenService.GenerateToken(user);
        return new AuthResponse(user.Id, token.AccessToken, token.ExpiresAtUtc);
    }
}
