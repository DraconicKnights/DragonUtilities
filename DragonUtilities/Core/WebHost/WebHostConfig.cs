using DragonUtilities.Interfaces;

namespace DragonUtilities.Core.WebHost;

public class WebHostConfig : IConfig
{
    public string WebRootPath { get; set; } = "wwwroot";
    public List<string> Prefixes { get; set; }
    public string DefaultFile { get; set; } = "index.html";
    public int MaxConnections { get; set; } = 100;
    public bool EnableLogging { get; set; } = true;
}