namespace TrueCodeExample.Finance.Application.Features.GetCurrencies;

public sealed record CurrencyResponse(
    string CharCode,
    string Name,
    int Nominal,
    decimal Value,
    DateTime UpdatedAtUtc);
