namespace TrueCodeExample.Finance.Application.Integration.UpsertCurrencies;

public sealed record CurrencyData(
    string CharCode,
    string NumCode,
    string Name,
    int Nominal,
    decimal Value,
    DateTime UpdatedAtUtc);
