namespace TrueCodeExample.Finance.Domain.Entities;

public class FavoriteCurrency
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid CurrencyId { get; private set; }

    private FavoriteCurrency()
    {
    }

    public FavoriteCurrency(Guid userId, Guid currencyId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CurrencyId = currencyId;
    }
}
