namespace TrueCodeExample.Finance.Application.Contracts;

public sealed record CurrencyData(
    string CharCode,
    string NumCode,
    string Name,
    int Nominal,
    decimal Value,
    DateTime UpdatedAtUtc);
