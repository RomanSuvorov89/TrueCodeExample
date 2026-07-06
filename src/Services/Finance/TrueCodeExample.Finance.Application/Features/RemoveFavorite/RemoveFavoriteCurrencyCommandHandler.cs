using Mediator;
using TrueCodeExample.Finance.Application.Features.AddFavorite;
using TrueCodeExample.Finance.Domain.Exceptions;

namespace TrueCodeExample.Finance.Application.Features.RemoveFavorite;

public sealed class RemoveFavoriteCurrencyCommandHandler(
    ICurrencyByCharCodeReader currencies,
    IFavoriteCurrencyRemover favorites)
    : IRequestHandler<RemoveFavoriteCurrencyCommand>
{
    public async ValueTask<Unit> Handle(RemoveFavoriteCurrencyCommand request, CancellationToken cancellationToken)
    {
        var charCode = request.CharCode.ToUpperInvariant();

        var currency = await currencies.GetByCharCodeAsync(charCode, cancellationToken)
                       ?? throw new CurrencyNotFoundException(charCode);

        var favorite = await favorites.GetAsync(request.UserId, currency.Id, cancellationToken)
                       ?? throw new FavoriteNotFoundException(charCode);

        favorites.Remove(favorite);
        await favorites.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
