using Mediator;
using TrueCodeExample.Finance.Application.Abstractions;
using TrueCodeExample.Finance.Domain.Entities;
using TrueCodeExample.Finance.Domain.Exceptions;

namespace TrueCodeExample.Finance.Application.Features.AddFavorite;

public sealed class AddFavoriteCurrencyCommandHandler(
    ICurrencyRepository currencies,
    IFavoriteCurrencyRepository favorites)
    : IRequestHandler<AddFavoriteCurrencyCommand>
{
    public async ValueTask<Unit> Handle(AddFavoriteCurrencyCommand request, CancellationToken cancellationToken)
    {
        var charCode = request.CharCode.ToUpperInvariant();

        var currency = await currencies.GetByCharCodeAsync(charCode, cancellationToken)
                       ?? throw new CurrencyNotFoundException(charCode);

        if (await favorites.ExistsAsync(request.UserId, currency.Id, cancellationToken))
        {
            throw new FavoriteAlreadyExistsException(charCode);
        }

        await favorites.AddAsync(new FavoriteCurrency(request.UserId, currency.Id), cancellationToken);
        await favorites.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
