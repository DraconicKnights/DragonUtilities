using System.Net;
using DragonUtilities.Attributes;
using DragonUtilities.Enums;
using DragonUtilities.Interfaces;

namespace DragonUtilities.Core.WebHost;

/// <summary>
/// Web Host Service
/// A service that manages an HTTP server to listen for and handle incoming requests.
/// </summary>
[RegisterInject]
public class WebHostService : IWebHost
{
    private readonly HttpListener _listener;
    private readonly WebHostConfig _config;
    
    [Inject]
    public ILogger Logger { get; set; }
    
    public WebHostService(WebHostConfig config)
    {
        if (config == null)
            throw new ArgumentNullException(nameof(config), "Configuration cannot be null.");
    
        _config = config;

        // Check if Prefixes is null or empty
        if (_config.Prefixes == null || !_config.Prefixes.Any())
            throw new ArgumentException("Prefixes must be provided in the configuration.");

        _listener = new HttpListener();

        foreach (var prefix in _config.Prefixes)
        {
            _listener.Prefixes.Add(prefix);
        }
    }

    /// <summary>
    /// Starts the HTTP listener to begin accepting incoming requests.
    /// </summary>
    public void Start()
    {
        if (!_listener.IsListening)
        {
            //_listener.Start();
            Log("Web server started on: " + string.Join(", ", _config.Prefixes), LogLevel.INFO);
            //Task.Run(HandleRequests);
        }
    }

    /// <summary>
    /// Stops the HTTP listener to stop accepting incoming requests.
    /// </summary>
    public void Stop()
    {
        if (_listener.IsListening)
        {
            _listener.Stop();
            Log("Web server stopped.", LogLevel.INFO);
        }
    }

    /// <summary>
    /// Restarts the HTTP server by stopping and starting it again.
    /// </summary>
    public void Restart()
    {
        Stop();
        Start();
        Log("Web server restarted.", LogLevel.INFO);
    }

    /// <summary>
    /// Handles incoming HTTP requests asynchronously.
    /// </summary>
    private async Task HandleRequests()
    {
        while (_listener.IsListening)
        {
            try
            {
                var context = await _listener.GetContextAsync();
                var response = context.Response;
                var requestPath = context.Request.Url?.LocalPath.TrimStart('/');
                var filePath = Path.Combine(_config.WebRootPath, requestPath!);

                // Serve the default file if the root is requested
                if (string.IsNullOrWhiteSpace(requestPath))
                {
                    filePath = Path.Combine(_config.WebRootPath, _config.DefaultFile);
                }

                if (File.Exists(filePath))
                {
                    var fileBytes = File.ReadAllBytes(filePath);
                    response.ContentType = GetMimeType(filePath);
                    response.ContentLength64 = fileBytes.Length;
                    await response.OutputStream.WriteAsync(fileBytes, 0, fileBytes.Length);
                    Log($"200 OK: {context.Request.Url}", LogLevel.INFO);
                }
                else
                {
                    response.StatusCode = 404;
                    var errorBytes = "404 - File Not Found"u8.ToArray();
                    await response.OutputStream.WriteAsync(errorBytes, 0, errorBytes.Length);
                    Log($"404 Not Found: {context.Request.Url}", LogLevel.INFO);
                }
                response.OutputStream.Close();
            }
            catch (Exception ex)
            {
                Log($"Error handling request: {ex.Message}", LogLevel.ERROR);
            }
        }
    }

    /// <summary>
    /// Determines the MIME type based on file extension.
    /// </summary>
    private string GetMimeType(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        return extension switch
        {
            ".html" => "text/html",
            ".css" => "text/css",
            ".js" => "application/javascript",
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            ".gif" => "image/gif",
            ".json" => "application/json",
            ".txt" => "text/plain",
            _ => "application/octet-stream"
        };
    }

    /// <summary>
    /// Logs messages with a specified log level.
    /// </summary>
    private void Log(string message, LogLevel logLevel)
    {
        if (_config.EnableLogging)
        {
            Logger.Log($"{message}", logLevel);
        }
    }
}