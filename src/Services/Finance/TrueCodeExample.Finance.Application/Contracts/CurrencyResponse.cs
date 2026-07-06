namespace TrueCodeExample.Finance.Application.Contracts;

public sealed record CurrencyResponse(
    string CharCode,
    string Name,
    int Nominal,
    decimal Value,
    DateTime UpdatedAtUtc);
