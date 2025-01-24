using System.ComponentModel;
using DragonUtilities.Enums;
using DragonUtilities.Interfaces;

namespace DragonUtilities.Core.Logging;

/// <summary>
/// Configuration settings for the Logger.
/// </summary>
public class LoggerConfig : IConfig
{
    /// <summary>
    /// Specifies the minimum log level for messages to be logged.
    /// Messages below this level will be ignored.
    /// Available levels:
    /// 0 - DEBUG: Detailed information, typically used for troubleshooting.
    /// 1 - INFO: General information about application operations.
    /// 2 - WARNING: Potential issues that are not errors but may require attention.
    /// 3 - ERROR: Errors that have occurred and need to be addressed.
    /// 4 - CRITICAL: Severe errors that may cause the application to fail.
    /// </summary>
    [Description("Minimum log level for messages to be recorded. Levels: 0 - DEBUG, 1 - INFO, 2 - WARNING, 3 - ERROR, 4 - CRITICAL.")]
    public LogLevel MinimumLogLevel { get; set; }
    
    /// <summary>
    /// A list of filters to exclude specific log messages based on their content.
    /// Messages matching any filter in this list will be ignored.
    /// </summary>
    [Description("Filters to exclude specific log messages based on their content.")]
    public List<string> Filters { get; set; }
    
    /// <summary>
    /// Indicates whether logs should be written to a file.
    /// </summary>
    [Description("Enables or disables file logging.")]
    public bool LogToFile { get; set; }
    
    /// <summary>
    /// Indicates whether logs should be written to the console.
    /// </summary>
    [Description("Enables or disables console logging.")]
    public bool LogToConsole { get; set; }
    
    /// <summary>
    /// The name of the log file where messages will be stored.
    /// Default value is "logs.txt".
    /// </summary>
    [Description("The name of the log file. Default is 'logs.txt'.")]
    public string? LogFileName { get; set; }
    
    /// <summary>
    /// The number of days to retain log files before they are deleted.
    /// </summary>
    [Description("The number of days to retain log files.")]
    public int LogRetentionDays { get; set; }
    
    /// <summary>
    /// The maximum size of a log file in bytes.
    /// When the file exceeds this size, it will be rotated.
    /// </summary>
    [Description("The maximum size of a log file in bytes before rotation.")]
    public long MaxLogFileSize { get; set; }
    
    /// <summary>
    /// Indicates whether each log message should include a timestamp.
    /// </summary>
    [Description("Enables or disables including timestamps in log messages.")]
    public bool IncludeTimestamp { get; set; }
}