using System.Text.Json.Serialization;

namespace Core.common;

public class ExchangeRatesResponse
{
    [JsonPropertyName("data")]
    public Dictionary<string, double> Data { get; set; }
}