namespace DragonUtilities.Core.Cache;

public class CacheStats
{
    public int CacheHits { get; set; }
    public int CacheMisses { get; set; }
    public int CacheEvictions { get; set; }
}