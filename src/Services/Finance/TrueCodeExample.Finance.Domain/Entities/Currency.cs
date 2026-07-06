namespace TrueCodeExample.Finance.Domain.Entities;

public class Currency
{
    public Guid Id { get; private set; }
    public string CharCode { get; private set; } = string.Empty;
    public string NumCode { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public int Nominal { get; private set; }
    public decimal Value { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }

    public Currency(string charCode, string numCode, string name, int nominal, decimal value, DateTime updatedAtUtc)
    {
        Id = Guid.NewGuid();
        CharCode = charCode;
        NumCode = numCode;
        Name = name;
        Nominal = nominal;
        Value = value;
        UpdatedAtUtc = updatedAtUtc;
    }

    private Currency(Guid id, string charCode, string numCode, string name, int nominal, decimal value, DateTime updatedAtUtc)
    {
        Id = id;
        CharCode = charCode;
        NumCode = numCode;
        Name = name;
        Nominal = nominal;
        Value = value;
        UpdatedAtUtc = updatedAtUtc;
    }

    public static Currency Restore(Guid id, string charCode, string numCode, string name, int nominal, decimal value, DateTime updatedAtUtc)
        => new(id, charCode, numCode, name, nominal, value, updatedAtUtc);

    public void Update(string numCode, string name, int nominal, decimal value, DateTime updatedAtUtc)
    {
        NumCode = numCode;
        Name = name;
        Nominal = nominal;
        Value = value;
        UpdatedAtUtc = updatedAtUtc;
    }
}
