using FluentAssertions;
using Moq;
using TrueCodeExample.Finance.Application.Features.AddFavorite;
using TrueCodeExample.Finance.Domain.Entities;
using TrueCodeExample.Finance.Domain.Exceptions;
using TrueCodeExample.Finance.Tests.Helpers;

namespace TrueCodeExample.Finance.Tests.Features.AddFavorite;

public sealed class AddFavoriteCurrencyCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenCurrencyExists_AddsFavorite()
    {
        var currencies = new Mock<ICurrencyByCharCodeReader>();
        var favorites = new Mock<IFavoriteCurrencyWriter>();
        var userId = Guid.NewGuid();
        var currency = TestCurrencies.Usd;

        currencies.Setup(x => x.GetByCharCodeAsync("USD", It.IsAny<CancellationToken>())).ReturnsAsync(currency);
        favorites.Setup(x => x.ExistsAsync(userId, currency.Id, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        favorites.Setup(x => x.AddAsync(It.IsAny<FavoriteCurrency>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        favorites.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var handler = new AddFavoriteCurrencyCommandHandler(currencies.Object, favorites.Object);

        await handler.Handle(new AddFavoriteCurrencyCommand(userId, "usd"), CancellationToken.None);

        favorites.Verify(x => x.AddAsync(
            It.Is<FavoriteCurrency>(f => f.UserId == userId && f.CurrencyId == currency.Id),
            It.IsAny<CancellationToken>()), Times.Once);
        favorites.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCurrencyNotFound_ThrowsCurrencyNotFoundException()
    {
        var currencies = new Mock<ICurrencyByCharCodeReader>();
        var favorites = new Mock<IFavoriteCurrencyWriter>();

        currencies.Setup(x => x.GetByCharCodeAsync("USD", It.IsAny<CancellationToken>())).ReturnsAsync((Currency?)null);

        var handler = new AddFavoriteCurrencyCommandHandler(currencies.Object, favorites.Object);

        var act = async () => await handler.Handle(new AddFavoriteCurrencyCommand(Guid.NewGuid(), "USD"), CancellationToken.None);

        await act.Should().ThrowAsync<CurrencyNotFoundException>();
        favorites.Verify(x => x.AddAsync(It.IsAny<FavoriteCurrency>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenFavoriteAlreadyExists_ThrowsFavoriteAlreadyExistsException()
    {
        var currencies = new Mock<ICurrencyByCharCodeReader>();
        var favorites = new Mock<IFavoriteCurrencyWriter>();
        var userId = Guid.NewGuid();
        var currency = TestCurrencies.Usd;

        currencies.Setup(x => x.GetByCharCodeAsync("USD", It.IsAny<CancellationToken>())).ReturnsAsync(currency);
        favorites.Setup(x => x.ExistsAsync(userId, currency.Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var handler = new AddFavoriteCurrencyCommandHandler(currencies.Object, favorites.Object);

        var act = async () => await handler.Handle(new AddFavoriteCurrencyCommand(userId, "USD"), CancellationToken.None);

        await act.Should().ThrowAsync<FavoriteAlreadyExistsException>();
        favorites.Verify(x => x.AddAsync(It.IsAny<FavoriteCurrency>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
