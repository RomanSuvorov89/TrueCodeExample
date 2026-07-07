using FluentValidation;

namespace TrueCodeExample.Finance.Application.Features.AddFavorite;

public sealed class AddFavoriteCurrencyCommandValidator : AbstractValidator<AddFavoriteCurrencyCommand>
{
    public AddFavoriteCurrencyCommandValidator()
    {
        RuleFor(x => x.CharCode)
            .NotEmpty()
            .Length(3)
            .Matches("^[A-Za-z]{3}$")
            .WithMessage("CharCode must be a 3-letter ISO currency code.");
    }
}
