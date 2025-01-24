using DragonUtilities.Core;
using DragonUtilities.Core.Cache;

namespace DragonUtilities.Interfaces;

/// <summary>
/// Cache Service
/// This service provides caching functionality for storing and retrieving data temporarily, with automatic expiration.
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Adds or updates an item in the cache with a specified expiration time and priority.
    /// </summary>
    void Set<T>(string key, T item, TimeSpan expiration, CacheItemPriority priority = CacheItemPriority.Normal);

    /// <summary>
    /// Retrieves an item from the cache, if it exists and has not expired.
    /// </summary>
    T? Get<T>(string key);

    /// <summary>
    /// Removes a specific item from the cache.
    /// </summary>
    void Remove(string key);

    /// <summary>
    /// Checks if a specific key exists in the cache and has not expired.
    /// </summary>
    bool Contains(string key);

    /// <summary>
    /// Sets dependencies between cache items, meaning removing one item will also remove the dependent items.
    /// </summary>
    void SetDependencies(string key, params string[] dependentKeys);

    /// <summary>
    /// Clears all items from the cache.
    /// </summary>
    void ClearCache();

    /// <summary>
    /// Retrieves statistics about cache hits and misses.
    /// </summary>
    CacheStats GetCacheStats();

    /// <summary>
    /// Adds or updates an item in the cache with a specified expiration time and priority asynchronously.
    /// </summary>
    Task SetAsync<T>(string key, T item, TimeSpan expiration, CacheItemPriority priority = CacheItemPriority.Normal);

    /// <summary>
    /// Retrieves an item from the cache, if it exists and has not expired asynchronously.
    /// </summary>
    Task<T?> GetAsync<T>(string key);

    /// <summary>
    /// Removes a specific item from the cache asynchronously.
    /// </summary>
    Task RemoveAsync(string key);

    /// <summary>
    /// Checks if a specific key exists in the cache and has not expired asynchronously.
    /// </summary>
    Task<bool> ContainsAsync(string key);
    event EventHandler<CacheEventArgs> CacheItemAdded;
    event EventHandler<CacheEventArgs> CacheItemUpdated;
    event EventHandler<CacheEventArgs> CacheItemRemoved;
}