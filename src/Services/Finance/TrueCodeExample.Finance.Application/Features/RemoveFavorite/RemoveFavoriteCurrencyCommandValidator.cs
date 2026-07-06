using FluentValidation;

namespace TrueCodeExample.Finance.Application.Features.RemoveFavorite;

public sealed class RemoveFavoriteCurrencyCommandValidator : AbstractValidator<RemoveFavoriteCurrencyCommand>
{
    public RemoveFavoriteCurrencyCommandValidator()
    {
        RuleFor(x => x.CharCode)
            .NotEmpty()
            .MaximumLength(3);
    }
}
