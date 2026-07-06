using FluentAssertions;
using Moq;
using TrueCodeExample.Finance.Application.Features.GetCurrencies;
using TrueCodeExample.Finance.Tests.Helpers;

namespace TrueCodeExample.Finance.Tests.Features.GetCurrencies;

public sealed class GetCurrenciesQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsCurrenciesSortedByCharCode()
    {
        var currencies = new Mock<ICurrencyListReader>();
        currencies.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { TestCurrencies.Usd, TestCurrencies.Eur });

        var handler = new GetCurrenciesQueryHandler(currencies.Object);

        var result = await handler.Handle(new GetCurrenciesQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
        result.Select(x => x.CharCode).Should().ContainInOrder("EUR", "USD");
        result.Should().ContainEquivalentOf(new CurrencyResponse("USD", "US Dollar", 1, 90.5m, TestCurrencies.Usd.UpdatedAtUtc));
    }
}
