namespace DragonUtilities.Core.Cache;

public class CacheEventArgs
{
    public string Key { get; set; }
    public CacheEventArgs(string key) => Key = key;
}