namespace TrueCodeExample.Finance.Application.Features.UpsertCurrencies;

public sealed record CurrencyData(
    string CharCode,
    string NumCode,
    string Name,
    int Nominal,
    decimal Value,
    DateTime UpdatedAtUtc);
