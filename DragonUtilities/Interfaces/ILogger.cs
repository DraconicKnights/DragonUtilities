using DragonUtilities.Enums;

namespace DragonUtilities.Interfaces;

/// <summary>
/// Interface for logging functionality.
/// Provides methods for logging messages, handling exceptions, and formatting log messages.
/// </summary>
public interface ILogger
{
    /// <summary>
    /// Logs a message with the specified log level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="logLevel">The severity level of the log message.</param>
    void Log(string message, LogLevel logLevel);

    /// <summary>
    /// Formats a log message with a prefix, timestamp, and optional colouring based on log level.
    /// </summary>
    /// <param name="message">The message to format.</param>
    /// <param name="logLevel">The severity level of the log message.</param>
    /// <returns>A formatted log message string.</returns>
    string FormatMessage(string message, LogLevel logLevel);

    /// <summary>
    /// Logs a debug message.
    /// </summary>
    /// <param name="message">The debug message to log.</param>
    void LogDebug(string message);

    /// <summary>
    /// Logs an informational message.
    /// </summary>
    /// <param name="message">The informational message to log.</param>
    void LogInfo(string message);

    /// <summary>
    /// Logs a warning message.
    /// </summary>
    /// <param name="message">The warning message to log.</param>
    void LogWarning(string message);

    /// <summary>
    /// Logs an error message.
    /// </summary>
    /// <param name="message">The error message to log.</param>
    void LogError(string message);

    /// <summary>
    /// Logs a critical error message.
    /// </summary>
    /// <param name="message">The critical error message to log.</param>
    void LogCritical(string message);

    /// <summary>
    /// Logs a debug message asynchronously.
    /// </summary>
    /// <param name="message">The debug message to log asynchronously.</param>
    void LogDebugAsync(string message);

    /// <summary>
    /// Logs an informational message asynchronously.
    /// </summary>
    /// <param name="message">The informational message to log asynchronously.</param>
    void LogInfoAsync(string message);

    /// <summary>
    /// Logs a warning message asynchronously.
    /// </summary>
    /// <param name="message">The warning message to log asynchronously.</param>
    void LogWarningAsync(string message);

    /// <summary>
    /// Logs an error message asynchronously.
    /// </summary>
    /// <param name="message">The error message to log asynchronously.</param>
    void LogErrorAsync(string message);

    /// <summary>
    /// Logs a critical error message asynchronously.
    /// </summary>
    /// <param name="message">The critical error message to log asynchronously.</param>
    void LogCriticalAsync(string message);

    /// <summary>
    /// Logs an exception, including the exception message and stack trace, as an error.
    /// </summary>
    /// <param name="ex">The exception to log.</param>
    void LogException(Exception ex);

    /// <summary>
    /// Logs a message with a timestamp and specified log level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="logLevel">The severity level of the log message.</param>
    void LogWithTimestamp(string message, LogLevel logLevel);

    /// <summary>
    /// Logs a message to a specified file with the given log level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="logLevel">The severity level of the log message.</param>
    /// <param name="filePath">The file path to write the log message to.</param>
    void LogToFile(string message, LogLevel logLevel, string filePath);
}