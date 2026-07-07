using System.Xml.Serialization;

namespace TrueCodeExample.CurrencyWorker.Cbr;

[XmlRoot("ValCurs")]
public sealed class CbrValCurs
{
    [XmlAttribute("Date")]
    public string Date { get; set; } = string.Empty;

    [XmlElement("Valute")]
    public List<CbrValute> Valutes { get; set; } = [];
}

public sealed class CbrValute
{
    [XmlElement("NumCode")]
    public string NumCode { get; set; } = string.Empty;

    [XmlElement("CharCode")]
    public string CharCode { get; set; } = string.Empty;

    [XmlElement("Nominal")]
    public int Nominal { get; set; }

    [XmlElement("Name")]
    public string Name { get; set; } = string.Empty;

    [XmlElement("Value")]
    public string Value { get; set; } = string.Empty;
}
