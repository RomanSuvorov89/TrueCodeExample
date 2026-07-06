using TrueCodeExample.Finance.Domain.Entities;

namespace TrueCodeExample.Finance.Tests.Helpers;

internal static class TestCurrencies
{
    public static Currency Usd => Currency.Restore(
        Guid.Parse("11111111-1111-1111-1111-111111111111"),
        "USD",
        "840",
        "US Dollar",
        1,
        90.5m,
        new DateTime(2026, 7, 6, 0, 0, 0, DateTimeKind.Utc));

    public static Currency Eur => Currency.Restore(
        Guid.Parse("22222222-2222-2222-2222-222222222222"),
        "EUR",
        "978",
        "Euro",
        1,
        98.2m,
        new DateTime(2026, 7, 6, 0, 0, 0, DateTimeKind.Utc));
}
