using TrueCodeExample.Finance.Domain.Entities;

namespace TrueCodeExample.Finance.DataAccess.Entities;

internal static class FinanceEntityMappings
{
    public static Currency ToDomain(this CurrencyEntity entity)
        => Currency.Restore(entity.Id, entity.CharCode, entity.NumCode, entity.Name, entity.Nominal, entity.Value, entity.UpdatedAtUtc);

    public static CurrencyEntity ToEntity(this Currency currency)
        => new()
        {
            Id = currency.Id,
            CharCode = currency.CharCode,
            NumCode = currency.NumCode,
            Name = currency.Name,
            Nominal = currency.Nominal,
            Value = currency.Value,
            UpdatedAtUtc = currency.UpdatedAtUtc
        };

    public static FavoriteCurrency ToDomain(this FavoriteCurrencyEntity entity)
        => FavoriteCurrency.Restore(entity.Id, entity.UserId, entity.CurrencyId);

    public static FavoriteCurrencyEntity ToEntity(this FavoriteCurrency favorite)
        => new()
        {
            Id = favorite.Id,
            UserId = favorite.UserId,
            CurrencyId = favorite.CurrencyId
        };
}
