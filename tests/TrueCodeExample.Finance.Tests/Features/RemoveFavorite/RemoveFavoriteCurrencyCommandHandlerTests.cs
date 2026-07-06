using FluentAssertions;
using Moq;
using TrueCodeExample.Finance.Application.Features.AddFavorite;
using TrueCodeExample.Finance.Application.Features.RemoveFavorite;
using TrueCodeExample.Finance.Domain.Entities;
using TrueCodeExample.Finance.Domain.Exceptions;
using TrueCodeExample.Finance.Tests.Helpers;

namespace TrueCodeExample.Finance.Tests.Features.RemoveFavorite;

public sealed class RemoveFavoriteCurrencyCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenFavoriteExists_RemovesFavorite()
    {
        var currencies = new Mock<ICurrencyByCharCodeReader>();
        var favorites = new Mock<IFavoriteCurrencyRemover>();
        var userId = Guid.NewGuid();
        var currency = TestCurrencies.Usd;
        var favorite = FavoriteCurrency.Restore(Guid.NewGuid(), userId, currency.Id);

        currencies.Setup(x => x.GetByCharCodeAsync("EUR", It.IsAny<CancellationToken>())).ReturnsAsync(currency);
        favorites.Setup(x => x.GetAsync(userId, currency.Id, It.IsAny<CancellationToken>())).ReturnsAsync(favorite);
        favorites.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var handler = new RemoveFavoriteCurrencyCommandHandler(currencies.Object, favorites.Object);

        await handler.Handle(new RemoveFavoriteCurrencyCommand(userId, "eur"), CancellationToken.None);

        favorites.Verify(x => x.Remove(favorite), Times.Once);
        favorites.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCurrencyNotFound_ThrowsCurrencyNotFoundException()
    {
        var currencies = new Mock<ICurrencyByCharCodeReader>();
        var favorites = new Mock<IFavoriteCurrencyRemover>();

        currencies.Setup(x => x.GetByCharCodeAsync("USD", It.IsAny<CancellationToken>())).ReturnsAsync((Currency?)null);

        var handler = new RemoveFavoriteCurrencyCommandHandler(currencies.Object, favorites.Object);

        var act = async () => await handler.Handle(new RemoveFavoriteCurrencyCommand(Guid.NewGuid(), "USD"), CancellationToken.None);

        await act.Should().ThrowAsync<CurrencyNotFoundException>();
    }

    [Fact]
    public async Task Handle_WhenFavoriteNotFound_ThrowsFavoriteNotFoundException()
    {
        var currencies = new Mock<ICurrencyByCharCodeReader>();
        var favorites = new Mock<IFavoriteCurrencyRemover>();
        var userId = Guid.NewGuid();
        var currency = TestCurrencies.Usd;

        currencies.Setup(x => x.GetByCharCodeAsync("USD", It.IsAny<CancellationToken>())).ReturnsAsync(currency);
        favorites.Setup(x => x.GetAsync(userId, currency.Id, It.IsAny<CancellationToken>())).ReturnsAsync((FavoriteCurrency?)null);

        var handler = new RemoveFavoriteCurrencyCommandHandler(currencies.Object, favorites.Object);

        var act = async () => await handler.Handle(new RemoveFavoriteCurrencyCommand(userId, "USD"), CancellationToken.None);

        await act.Should().ThrowAsync<FavoriteNotFoundException>();
        favorites.Verify(x => x.Remove(It.IsAny<FavoriteCurrency>()), Times.Never);
    }
}
