using System.Text.Json;
using System.Web;

namespace Infrastructure.HttpClientS;

public class ExchangeRatesAppHttpClient : IAppHttpClient
{
    private readonly HttpClient _httpClient;
    private const string _apiKey = "fca_live_p27fskZlm6biYxKrLsa75BiPg4UVlPRC7QaBlwlt";
    
    public ExchangeRatesAppHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.freecurrencyapi.com/v1/latest");
    }

    public async Task<T?> GetAsJsonAsync<T>(Dictionary<string, string>? queryParams = null)
    {
        var fullQuery = BuildUrlWithParams(queryParams);

        var response = await _httpClient.GetAsync(fullQuery);
        var responseContent = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<T>(responseContent);
    }

    private string BuildUrlWithParams(Dictionary<string, string>? queryParams)
    {
        var builder = new UriBuilder(_httpClient.BaseAddress!);
        var query = HttpUtility.ParseQueryString(builder.Query);

        query["apikey"] = _apiKey;
        
        if (queryParams is not null)
        {
            foreach (var param in queryParams)
            {
                query[param.Key] = param.Value;
            }
        }

        builder.Query = query.ToString();
        return builder.ToString();
    }
}