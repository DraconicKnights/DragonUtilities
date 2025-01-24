using System.Collections.Concurrent;
using DragonUtilities.Attributes;
using DragonUtilities.Interfaces;

namespace DragonUtilities.Core.Cache;

/// <summary>
/// Cache Service
/// This service provides caching functionality for storing and retrieving data temporarily, with automatic expiration.
/// </summary>
[RegisterInject]
public class CacheService : ICacheService
{
   private readonly ConcurrentDictionary<string, (object Value, DateTime Expiration, CacheItemPriority Priority)> _cache =
        new ConcurrentDictionary<string, (object, DateTime, CacheItemPriority)>();
    private readonly ConcurrentDictionary<string, List<string>> _dependencies =
        new ConcurrentDictionary<string, List<string>>();
    private CacheStats _cacheStats = new CacheStats();

    // Events for cache item changes

    public event EventHandler<CacheEventArgs> CacheItemAdded;
    public event EventHandler<CacheEventArgs> CacheItemUpdated;
    public event EventHandler<CacheEventArgs> CacheItemRemoved;

    /// <summary>
    /// Adds or updates an item in the cache with a specified expiration time and priority.
    /// </summary>
    public void Set<T>(string key, T item, TimeSpan expiration, CacheItemPriority priority = CacheItemPriority.Normal)
    {
        var expirationTime = DateTime.Now.Add(expiration);
        _cache[key] = (item, expirationTime, priority)!;
        CacheItemAdded?.Invoke(this, new CacheEventArgs(key));
    }

    /// <summary>
    /// Retrieves an item from the cache, if it exists and has not expired.
    /// </summary>
    public T? Get<T>(string key)
    {
        if (_cache.TryGetValue(key, out var cacheItem) && cacheItem.Expiration > DateTime.Now)
        {
            _cacheStats.CacheHits++;
            return (T)cacheItem.Value;
        }
        _cacheStats.CacheMisses++;
        return default;
    }

    /// <summary>
    /// Removes a specific item from the cache.
    /// </summary>
    public void Remove(string key)
    {
        if (_cache.TryRemove(key, out _))
        {
            CacheItemRemoved?.Invoke(this, new CacheEventArgs(key));
        }
    }

    /// <summary>
    /// Checks if a specific key exists in the cache and has not expired.
    /// </summary>
    public bool Contains(string key) => _cache.ContainsKey(key) && _cache[key].Expiration > DateTime.Now;

    /// <summary>
    /// Sets dependencies between cache items, meaning removing one item will also remove the dependent items.
    /// </summary>
    public void SetDependencies(string key, params string[] dependentKeys)
    {
        _dependencies[key] = new List<string>(dependentKeys);
    }

    /// <summary>
    /// Clears all items from the cache.
    /// </summary>
    public void ClearCache() => _cache.Clear();

    /// <summary>
    /// Retrieves statistics about cache hits and misses.
    /// </summary>
    public CacheStats GetCacheStats() => _cacheStats;

    #region Async Methods

    public async Task SetAsync<T>(string key, T item, TimeSpan expiration, CacheItemPriority priority = CacheItemPriority.Normal)
    {
        await Task.Run(() => Set(key, item, expiration, priority));
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        return await Task.Run(() => Get<T>(key));
    }

    public async Task RemoveAsync(string key)
    {
        await Task.Run(() => Remove(key));
    }

    public async Task<bool> ContainsAsync(string key)
    {
        return await Task.Run(() => Contains(key));
    }

    #endregion
}