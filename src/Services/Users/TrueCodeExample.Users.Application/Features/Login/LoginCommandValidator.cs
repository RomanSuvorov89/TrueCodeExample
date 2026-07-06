using FluentValidation;

namespace TrueCodeExample.Users.Application.Features.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}
