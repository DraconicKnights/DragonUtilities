using DragonUtilities.Attributes;
using DragonUtilities.Enums;
using DragonUtilities.Interfaces;

namespace DragonUtilities.Core.Logging;

/// <summary>
/// Logger service that provides logging functionality to various destinations (e.g., file, console).
/// Allows for synchronous and asynchronous logging with different log levels and formats.
/// </summary>
[RegisterInject]
public class Logger : ILogger
{
    private readonly List<ILogDestination> _logDestinations = new List<ILogDestination>();
    private LogLevel MinimumLogLevel { get; set; }
    private List<string> Filters { get; set; }
    private LoggerConfig _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="Logger"/> class.
    /// </summary>
    /// <param name="config">The configuration settings for the logger (e.g., log level, log file, console logging).</param>
    public Logger(LoggerConfig config)
    {
        _config = config;
        MinimumLogLevel = config.MinimumLogLevel;
        Filters = config.Filters;

        if (config.LogToFile)
        {
            var fileLogDestination = new FileLogDestination(config.LogFileName, config);
            _logDestinations.Add(fileLogDestination);
            DragonUtilityManager.Instance.DebugLog($"$\"File logging enabled. Output file: {{config.LogFileName}}\"");
        }

        if (config.LogToConsole)
        {
            var consoleLogDestination = new ConsoleLogDestination();
            _logDestinations.Add(consoleLogDestination);
            DragonUtilityManager.Instance.DebugLog("Console logging enabled.");
        }
    }

    /// <summary>
    /// Logs a message synchronously to the configured destinations if the log level meets the minimum requirement.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="logLevel">The log level (e.g., DEBUG, INFO, ERROR).</param>
    public void Log(string message, LogLevel logLevel)
    {
        if (logLevel < MinimumLogLevel) return;
        var formattedMessage = FormatMessage(message, logLevel);
        foreach (var destination in _logDestinations)
        {
            destination.WriteLog(formattedMessage);
        }
    }

    /// <summary>
    /// Logs a message asynchronously to the configured destinations if the log level meets the minimum requirement.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="logLevel">The log level (e.g., DEBUG, INFO, ERROR).</param>
    public async Task LogAsync(string message, LogLevel logLevel)
    {
        if (logLevel >= MinimumLogLevel)
        {
            var formattedMessage = FormatMessage(message, logLevel);
            var logTasks =
                _logDestinations.Select(destination => Task.Run (() => destination.WriteLog(formattedMessage)));
            await Task.WhenAll(logTasks);
        }
    }

    /// <summary>
    /// Formats the log message with the appropriate log level and timestamp (if configured).
    /// </summary>
    /// <param name="message">The message to format.</param>
    /// <param name="logLevel">The log level to prepend to the message.</param>
    /// <returns>The formatted log message as a string.</returns>
    public string FormatMessage(string message, LogLevel logLevel)
    {
        var logPrefix = logLevel switch
        {
            LogLevel.DEBUG => "[DEBUG]",
            LogLevel.INFO => "[INFO]",
            LogLevel.WARNING => "[WARNING]",
            LogLevel.ERROR => "[ERROR]",
            LogLevel.CRITICAL => "[CRITICAL]",
            _ => "[UNKNOWN]"
        };

        var colourCode = logLevel switch
        {
            LogLevel.DEBUG => "\u001b[34m", // Blue
            LogLevel.INFO => "\u001b[32m", // Green
            LogLevel.WARNING => "\u001b[33m", // Yellow
            LogLevel.ERROR => "\u001b[31m", // Red
            LogLevel.CRITICAL => "\u001b[35m", // Magenta
            _ => "\u001b[0m" // Reset
        };

        var timestamp = _config.IncludeTimestamp ? $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} " : string.Empty;
        var textCode = "\u001b[0m";

        return $"{colourCode}{logPrefix} {timestamp}{textCode}{message}";
    }

    /// <summary>
    /// Logs a message with the DEBUG log level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public void LogDebug(string message) => Log(message, LogLevel.DEBUG);

    /// <summary>
    /// Logs a message with the INFO log level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public void LogInfo(string message) => Log(message, LogLevel.INFO);

    /// <summary>
    /// Logs a message with the WARNING log level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public void LogWarning(string message) => Log(message, LogLevel.WARNING);

    /// <summary>
    /// Logs a message with the ERROR log level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public void LogError(string message) => Log(message, LogLevel.ERROR);

    /// <summary>
    /// Logs a message with the CRITICAL log level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public void LogCritical(string message) => Log(message, LogLevel.CRITICAL);

    /// <summary>
    /// Asynchronously logs a message with the DEBUG log level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public async void LogDebugAsync(string message) => await LogAsync(message, LogLevel.DEBUG);

    /// <summary>
    /// Asynchronously logs a message with the INFO log level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public async void LogInfoAsync(string message) => await LogAsync(message, LogLevel.INFO);

    /// <summary>
    /// Asynchronously logs a message with the WARNING log level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public async void LogWarningAsync(string message) => await LogAsync(message, LogLevel.WARNING);

    /// <summary>
    /// Asynchronously logs a message with the ERROR log level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public async void LogErrorAsync(string message) => await LogAsync(message, LogLevel.ERROR);

    /// <summary>
    /// Asynchronously logs a message with the CRITICAL log level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public async void LogCriticalAsync(string message) => await LogAsync(message, LogLevel.CRITICAL);

    /// <summary>
    /// Logs an exception by logging its message and stack trace with the ERROR log level.
    /// </summary>
    /// <param name="ex">The exception to log.</param>
    public void LogException(Exception ex)
    {
        LogError($"Exception: {ex.Message}");
        LogError($"Stack Trace: {ex.StackTrace}");
    }

    /// <summary>
    /// Logs a message with a timestamp included, based on the configured format.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="logLevel">The log level to associate with the message.</param>
    public void LogWithTimestamp(string message, LogLevel logLevel)
    {
        string timeStampedMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
        Log(timeStampedMessage, logLevel);
    }

    /// <summary>
    /// Logs a message to a file with a specific log level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="logLevel">The log level to associate with the message.</param>
    /// <param name="filePath">The file path where the log will be written.</param>
    public void LogToFile(string message, LogLevel logLevel, string filePath)
    {
        string formattedMessage = FormatMessage(message, logLevel);
        File.AppendAllText(filePath, formattedMessage + Environment.NewLine);
    }
}