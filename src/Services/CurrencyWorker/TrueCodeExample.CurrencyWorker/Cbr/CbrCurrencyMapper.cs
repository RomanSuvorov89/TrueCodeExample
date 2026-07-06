using System.Globalization;
using System.Text;
using System.Xml.Serialization;
using TrueCodeExample.Finance.Application.Features.UpsertCurrencies;

namespace TrueCodeExample.CurrencyWorker.Cbr;

public static class CbrCurrencyMapper
{
    private static readonly Encoding CbrEncoding = Encoding.GetEncoding(1251);

    public static async Task<CbrValCurs> ParseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream, CbrEncoding);

        var serializer = new XmlSerializer(typeof(CbrValCurs));
        return (CbrValCurs)serializer.Deserialize(reader)!;
    }

    public static IReadOnlyCollection<CurrencyData> ToCurrencyData(CbrValCurs valCurs)
    {
        var updatedAtUtc = ParseDate(valCurs.Date);

        return valCurs.Valutes
            .Where(v => !string.IsNullOrWhiteSpace(v.CharCode))
            .Select(v => new CurrencyData(
                v.CharCode.Trim(),
                v.NumCode.Trim(),
                v.Name.Trim(),
                v.Nominal,
                ParseValue(v.Value),
                updatedAtUtc))
            .ToList();
    }

    private static DateTime ParseDate(string date)
    {
        var parsed = DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture);
        return DateTime.SpecifyKind(parsed, DateTimeKind.Utc);
    }

    private static decimal ParseValue(string value)
    {
        var normalized = value.Trim().Replace(',', '.');
        return decimal.Parse(normalized, CultureInfo.InvariantCulture);
    }
}
