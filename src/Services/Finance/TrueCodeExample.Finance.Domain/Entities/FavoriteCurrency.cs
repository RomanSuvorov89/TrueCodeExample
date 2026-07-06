namespace TrueCodeExample.Finance.Domain.Entities;

public class FavoriteCurrency
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid CurrencyId { get; private set; }

    public FavoriteCurrency(Guid userId, Guid currencyId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CurrencyId = currencyId;
    }

    private FavoriteCurrency(Guid id, Guid userId, Guid currencyId)
    {
        Id = id;
        UserId = userId;
        CurrencyId = currencyId;
    }

    public static FavoriteCurrency Restore(Guid id, Guid userId, Guid currencyId)
        => new(id, userId, currencyId);
}
