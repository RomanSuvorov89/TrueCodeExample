using FluentAssertions;
using Moq;
using TrueCodeExample.Finance.Application.Features.GetRates;
using TrueCodeExample.Finance.Domain.Entities;
using TrueCodeExample.Finance.Tests.Helpers;

namespace TrueCodeExample.Finance.Tests.Features.GetRates;

public sealed class GetUserRatesQueryHandlerTests
{
    [Fact]
    public async Task Handle_WhenUserHasNoFavorites_ReturnsEmptyList()
    {
        var favorites = new Mock<IFavoriteCurrencyByUserReader>();
        var currencies = new Mock<ICurrencyByIdsReader>();
        var userId = Guid.NewGuid();

        favorites.Setup(x => x.GetByUserAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync([]);

        var handler = new GetUserRatesQueryHandler(favorites.Object, currencies.Object);

        var result = await handler.Handle(new GetUserRatesQuery(userId), CancellationToken.None);

        result.Should().BeEmpty();
        currencies.Verify(x => x.GetByIdsAsync(It.IsAny<IReadOnlyCollection<Guid>>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUserHasFavorites_ReturnsFavoriteCurrenciesSortedByCharCode()
    {
        var favorites = new Mock<IFavoriteCurrencyByUserReader>();
        var currencies = new Mock<ICurrencyByIdsReader>();
        var userId = Guid.NewGuid();
        var usd = TestCurrencies.Usd;
        var eur = TestCurrencies.Eur;

        favorites.Setup(x => x.GetByUserAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[]
            {
                FavoriteCurrency.Restore(Guid.NewGuid(), userId, usd.Id),
                FavoriteCurrency.Restore(Guid.NewGuid(), userId, eur.Id)
            });

        currencies.Setup(x => x.GetByIdsAsync(It.IsAny<IReadOnlyCollection<Guid>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { usd, eur });

        var handler = new GetUserRatesQueryHandler(favorites.Object, currencies.Object);

        var result = await handler.Handle(new GetUserRatesQuery(userId), CancellationToken.None);

        result.Should().HaveCount(2);
        result.Select(x => x.CharCode).Should().ContainInOrder("EUR", "USD");
    }
}
