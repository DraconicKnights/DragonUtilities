using DragonUtilities.Attributes;
using DragonUtilities.Interfaces;

namespace DragonUtilities.Core.Logging;

[Log]
public class ConsoleLogDestination : ILogDestination
{
    public void WriteLog(string message)
    {
        Console.WriteLine(message);
    }
}