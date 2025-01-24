namespace DragonUtilities.Interfaces;

/// <summary>
/// HTTP Client Service
/// A service that facilitates asynchronous HTTP requests for GET, POST, PUT, and DELETE operations.
/// </summary>
public interface IHTTPClientService
{
    /// <summary>
    /// Sends an HTTP GET request and returns the response content.
    /// </summary>
    Task<string> GetAsync(string url);

    /// <summary>
    /// Sends an HTTP POST request with content and returns the response content.
    /// </summary>
    Task<string> PostAsync(string url, HttpContent content);

    /// <summary>
    /// Sends an HTTP PUT request with content and returns the response content.
    /// </summary>
    Task<string> PutAsync(string url, HttpContent content);

    /// <summary>
    /// Sends an HTTP DELETE request and returns the response content.
    /// </summary>
    Task<string> DeleteAsync(string url);
}