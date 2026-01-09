using BonProf.Models;
using BonProf.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using BonProf.Utilities;
using System.IO;
using System.Web;

namespace BonProf.Services;

public class TokenService : ITokenService
{
    public static string FilerToken = "";
    private readonly HttpClient _httpClient;
    private readonly string _filerUrl;
    private readonly ILogger<TokenService> _logger;

    public TokenService(HttpClient httpClient, ILogger<TokenService> logger)
    {
        _httpClient = httpClient;
        _filerUrl = EnvironmentVariables.FilerUrl ?? throw new ArgumentNullException("FilerUrl is missing");
        _logger = logger;
    }

    public async Task GetAsync(string serviceName)
    {
        try
        {
            var uri = new UriBuilder($"{_filerUrl}/auth");
            var query = HttpUtility.ParseQueryString(uri.Query);
            query["serviceName"] = serviceName;
            uri.Query = query.ToString();

            _logger.LogInformation("Fetching token from: {Uri}", uri.Uri);

            using var response = await _httpClient.GetAsync(uri.Uri);
            response.EnsureSuccessStatusCode();

            var bodyAsText = await response.Content.ReadAsStringAsync();
            var bodyAsClass = System.Text.Json.JsonSerializer.Deserialize<FilerAuthResponse>(bodyAsText) 
                              ?? throw new Exception("Token deserialization failed");

            FilerToken = bodyAsClass.Token;
            _logger.LogInformation("Token fetched successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch token for service {ServiceName}", serviceName);
            throw;
        }
    }

    public async Task RefreshAsync(string serviceName)
    {
        _logger.LogInformation("Refreshing token for service {ServiceName}", serviceName);
        // TODO uncomment this
        // await GetAsync(serviceName); 
    }
}