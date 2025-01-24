// Copyright (c) 2025 DraconicKnights
// Licensed under the MIT License. See LICENSE file for details.

using DragonUtilities.Core.Logging;
using DragonUtilities.Core.WebHost;
using DragonUtilities.Enums;
using DragonUtilities.Handler;
using DragonUtilities.Utils;

namespace DragonUtilities;

/// <summary>
/// Utilities for managing services, dependencies, and configurations.
/// </summary>
public class DragonUtilityManager
{
    private static readonly Lazy<DragonUtilityManager> _instance = new(() => new DragonUtilityManager());
    private readonly Container _container;
    
    /// <summary>
    /// Singleton instance of DragonUtilities.
    /// </summary>
    public static DragonUtilityManager Instance => _instance.Value;

    private DebugConfig _debugConfig;

    /// <summary>
    /// Initializes the DragonUtilities class and registers dependencies.
    /// </summary>
    public DragonUtilityManager()
    {
        _container = new Container();
        RegisterDependencies();
    }

    /// <summary>
    /// Registers required dependencies and configuration settings.
    /// </summary>
    private void RegisterDependencies()
    {
        _container.Register(typeof(Container), _container.GetType());

        // Load and register configuration settings
        var loggerConfig = ConfigLoader.Load("Config", "LoggerConfig.json", GetDefaultLoggerConfig);
        var webHostConfig = ConfigLoader.Load("Config", "WebHostConfig.json", GetDefaultWebHostConfig);
        _debugConfig = ConfigLoader.Load("Config", "DragonUtils.json", GetDefaultDebugConfig);

        _container.RegisterInstance(loggerConfig);
        _container.RegisterInstance(webHostConfig);
    }
    
    /// <summary>
    /// Injects dependencies into the specified service instance.
    /// </summary>
    /// <param name="instance">The service instance to inject dependencies into.</param>
    /// <exception cref="ArgumentNullException">Thrown if the instance is null.</exception>
    public void ServiceFactory(object instance)
    {
        if (instance == null) throw new ArgumentNullException(nameof(instance));
        _container.InjectDependenciesIntoObject(instance);
    }

    /// <summary>
    /// Creates and returns an instance of the specified service type with dependencies injected.
    /// </summary>
    /// <typeparam name="T">The type of service to create.</typeparam>
    /// <returns>An instance of the specified service type.</returns>
    public T CreateService<T>() where T : class, new()
    {
        var instance = new T();
        ServiceFactory(instance);
        return instance;
    }
    
    /// <summary>
    /// Logs a debug message to the console if debug mode is enabled.
    /// </summary>
    /// <param name="message">The debug message to log.</param>
    public void DebugLog(string message)
    {
        if (_debugConfig.DebugMode)
        {
            Console.ForegroundColor = ConsoleColor.Cyan; // Aqua/Cyan
            Console.WriteLine($"[DEBUG] {message}");
            Console.ResetColor();
        }
    }

    // Default configuration generators
    private LoggerConfig GetDefaultLoggerConfig() => new()
    {
        MinimumLogLevel = LogLevel.INFO,
        Filters = [],
        LogToFile = false,
        LogToConsole = true,
        LogFileName = "Logs.log",
        MaxLogFileSize = 5 * 1024 * 1024,
        LogRetentionDays = 7,
        IncludeTimestamp = true
    };

    private WebHostConfig GetDefaultWebHostConfig() => new()
    {
        WebRootPath = "wwwroot",
        Prefixes = new List<string> { "http://localhost:8080/" },
        DefaultFile = "index.html",
        MaxConnections = 100,
        EnableLogging = true
    };
    
    // Default debug configuration generator
    private DebugConfig GetDefaultDebugConfig() => new()
    {
        DebugMode = false
    };
}
