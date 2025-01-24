using DragonUtilities.Attributes;
using DragonUtilities.Interfaces;

namespace DragonUtilities.Controller;

/// <summary>
/// Base Controller Class
/// This is the foundational utility controller that can be inherited for access to common services.
/// </summary>
public class BaseController
{
    /// <summary>
    /// ILogger Service
    /// A flexible and configurable logging service for recording system events and operations.
    /// </summary>
    [Inject]
    public ILogger Logger { get; set; }

    /// <summary>
    /// File service that assists with reading, writing, appending, and deleting files.
    /// It provides an easy interface for file operations and handling file content.
    /// </summary>
    [Inject]
    public IFileService FileService { get; set; }

    /// <summary>
    /// IHTTPClientService
    /// A service that handles making HTTP requests to external services.
    /// </summary>
    [Inject]
    public IHTTPClientService HttpClientService { get; set; }

    /// <summary>
    /// IWebHost Service
    /// A service used for the creation and management of an HTTP server.
    /// </summary>
    [Inject]
    public IWebHost WebHost { get; set; }

    /// <summary>
    /// ICacheService
    /// A service that handles caching to improve performance by storing data temporarily.
    /// </summary>
    [Inject]
    public ICacheService CacheService { get; set; }

    public BaseController()
    {
        DragonUtilityManager.Instance.ServiceFactory(this);
        Logger.LogInfo($"Base Controller has been synced to type: {GetType().Name}");
    }
}