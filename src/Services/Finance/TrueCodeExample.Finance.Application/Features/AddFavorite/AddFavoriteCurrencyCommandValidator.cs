using FluentValidation;

namespace TrueCodeExample.Finance.Application.Features.AddFavorite;

public sealed class AddFavoriteCurrencyCommandValidator : AbstractValidator<AddFavoriteCurrencyCommand>
{
    public AddFavoriteCurrencyCommandValidator()
    {
        RuleFor(x => x.CharCode)
            .NotEmpty()
            .MaximumLength(3);
    }
}
