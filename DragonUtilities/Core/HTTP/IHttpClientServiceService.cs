using DragonUtilities.Attributes;
using DragonUtilities.Interfaces;

namespace DragonUtilities.Core.HTTP;

/// <summary>
/// HTTP Client Service
/// A service that facilitates asynchronous HTTP requests for GET, POST, PUT, and DELETE operations.
/// </summary>
[RegisterInject]
public class HttpClientServiceService : IHTTPClientService
{
    private readonly HttpClient _httpClient = new HttpClient();

    /// <summary>
    /// Sends an HTTP GET request and returns the response content.
    /// </summary>
    public async Task<string> GetAsync(string url)
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// Sends an HTTP POST request with content and returns the response content.
    /// </summary>
    public async Task<string> PostAsync(string url, HttpContent content)
    {
        var response = await _httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// Sends an HTTP PUT request with content and returns the response content.
    /// </summary>
    public async Task<string> PutAsync(string url, HttpContent content)
    {
        var response = await _httpClient.PutAsync(url, content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// Sends an HTTP DELETE request and returns the response content.
    /// </summary>
    public async Task<string> DeleteAsync(string url)
    {
        var response = await _httpClient.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}