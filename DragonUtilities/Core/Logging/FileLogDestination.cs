using DragonUtilities.Attributes;
using DragonUtilities.Interfaces;

namespace DragonUtilities.Core.Logging;

[Log]
public class FileLogDestination : ILogDestination
{
    public string FilePath { get; }
    private readonly string _directory;
    private readonly LoggerConfig _config;

    public FileLogDestination(string? filePath, LoggerConfig config)
    {
        _config = config;

        _directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        EnsureLogDirectoryExists();

        // Force the file path to always reside within the Logs directory
        var logFileName = string.IsNullOrEmpty(filePath) ? (_config.LogFileName ?? "log.txt") : Path.GetFileName(filePath);
        FilePath = Path.Combine(_directory, logFileName);
    }

    public void WriteLog(string message)
    {
        EnsureLogDirectoryExists();

        if (!File.Exists(FilePath))
        {
            File.WriteAllText(FilePath, string.Empty);
        }

        RotateLogFileIfNecessary();

        File.AppendAllText(FilePath, message + Environment.NewLine);

        ManageLogFileRetention();
    }

    private void RotateLogFileIfNecessary()
    {
        if (_config.MaxLogFileSize > 0)
        {
            var fileInfo = new FileInfo(FilePath);

            if (fileInfo.Length > _config.MaxLogFileSize)
            {
                var rotatedFilePath = $"{FilePath}.{DateTime.Now:yyyyMMddHHmmss}";
                File.Move(FilePath, rotatedFilePath);

                File.WriteAllText(FilePath, string.Empty);
            }
        }
    }

    private void EnsureLogDirectoryExists()
    {
        if (!Directory.Exists(_directory))
        {
            Directory.CreateDirectory(_directory);
        }
    }

    private void ManageLogFileRetention()
    {
        if (_config.LogRetentionDays > 0)
        {
            var logFiles = Directory.GetFiles(_directory, "*.txt");

            foreach (var logFile in logFiles)
            {
                var fileInfo = new FileInfo(logFile);

                if (fileInfo.CreationTime < DateTime.Now.AddDays(-_config.LogRetentionDays))
                {
                    File.Delete(logFile);
                }
            }
        }
    }
}