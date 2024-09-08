namespace Infrastructure.HttpClientS;

public interface IAppHttpClient
{
    Task<T?> GetAsJsonAsync<T>(Dictionary<string, string>? queryParams);
}