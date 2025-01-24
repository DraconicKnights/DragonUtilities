using DragonUtilities.Interfaces;

namespace DragonUtilities.Utils;

public class DebugConfig : IConfig
{
    public bool DebugMode { get; set; } = false; //Default is disabled
}