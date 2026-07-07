using FluentAssertions;
using Moq;
using TrueCodeExample.Finance.Application.Integration.UpsertCurrencies;
using TrueCodeExample.Finance.Domain.Entities;
using TrueCodeExample.Finance.Tests.Helpers;

namespace TrueCodeExample.Finance.Tests.Features.UpsertCurrencies;

public sealed class UpsertCurrenciesCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenCollectionIsEmpty_DoesNothing()
    {
        var currencies = new Mock<ICurrencyUpsertStore>();
        var handler = new UpsertCurrenciesCommandHandler(currencies.Object);

        await handler.Handle(new UpsertCurrenciesCommand([]), CancellationToken.None);

        currencies.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Never);
        currencies.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenCurrencyDoesNotExist_AddsCurrency()
    {
        var currencies = new Mock<ICurrencyUpsertStore>();
        var updatedAt = new DateTime(2026, 7, 6, 0, 0, 0, DateTimeKind.Utc);
        var data = new CurrencyData("USD", "840", "US Dollar", 1, 90.5m, updatedAt);

        currencies.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync([]);
        currencies.Setup(x => x.AddAsync(It.IsAny<Currency>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        currencies.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var handler = new UpsertCurrenciesCommandHandler(currencies.Object);

        await handler.Handle(new UpsertCurrenciesCommand([data]), CancellationToken.None);

        currencies.Verify(x => x.AddAsync(
            It.Is<Currency>(c =>
                c.CharCode == "USD" &&
                c.NumCode == "840" &&
                c.Name == "US Dollar" &&
                c.Nominal == 1 &&
                c.Value == 90.5m &&
                c.UpdatedAtUtc == updatedAt),
            It.IsAny<CancellationToken>()), Times.Once);
        currencies.Verify(x => x.UpdateAsync(It.IsAny<Currency>(), It.IsAny<CancellationToken>()), Times.Never);
        currencies.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCurrencyExists_UpdatesCurrency()
    {
        var currencies = new Mock<ICurrencyUpsertStore>();
        var existing = TestCurrencies.Usd;
        var updatedAt = new DateTime(2026, 7, 7, 0, 0, 0, DateTimeKind.Utc);
        var data = new CurrencyData("USD", "840", "US Dollar updated", 1, 91.0m, updatedAt);

        currencies.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync([existing]);
        currencies.Setup(x => x.UpdateAsync(existing, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        currencies.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var handler = new UpsertCurrenciesCommandHandler(currencies.Object);

        await handler.Handle(new UpsertCurrenciesCommand([data]), CancellationToken.None);

        existing.Name.Should().Be("US Dollar updated");
        existing.Value.Should().Be(91.0m);
        existing.UpdatedAtUtc.Should().Be(updatedAt);
        currencies.Verify(x => x.UpdateAsync(existing, It.IsAny<CancellationToken>()), Times.Once);
        currencies.Verify(x => x.AddAsync(It.IsAny<Currency>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
