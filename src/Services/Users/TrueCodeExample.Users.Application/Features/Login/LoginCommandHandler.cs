using Mediator;
using TrueCodeExample.Users.Application.Services.AuthTokenIssuer;
using TrueCodeExample.Users.Application.DTO;
using TrueCodeExample.Users.Domain.Exceptions;

namespace TrueCodeExample.Users.Application.Features.Login;

public sealed class LoginCommandHandler(
    ILoginUserStore users,
    ILoginPasswordVerifier passwordVerifier,
    TokenIssuer tokenIssuer)
    : IRequestHandler<LoginCommand, AuthResponse>
{
    public async ValueTask<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await users.GetByNameAsync(request.Name, cancellationToken);

        if (user is null || !passwordVerifier.Verify(request.Password, user.PasswordHash))
        {
            throw new InvalidCredentialsException();
        }

        return await tokenIssuer.IssueAsync(user, cancellationToken);
    }
}
