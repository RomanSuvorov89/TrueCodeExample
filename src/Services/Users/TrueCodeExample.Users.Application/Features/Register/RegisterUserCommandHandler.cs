using Mediator;
using TrueCodeExample.Users.Application.Services.AuthTokenIssuer;
using TrueCodeExample.Users.Application.DTO;
using TrueCodeExample.Users.Domain.Entities;
using TrueCodeExample.Users.Domain.Exceptions;
namespace TrueCodeExample.Users.Application.Features.Register;

public sealed class RegisterUserCommandHandler(
    IRegisterUserStore users,
    IRegisterPasswordHasher passwordHasher,
    TokenIssuer tokenIssuer)
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

        return await tokenIssuer.IssueAsync(user, cancellationToken);
    }
}
