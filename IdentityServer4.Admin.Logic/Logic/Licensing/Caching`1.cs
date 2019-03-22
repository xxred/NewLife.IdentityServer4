





using Microsoft.Extensions.Caching.Memory;
using System;

namespace IdentityServer4.Admin.Logic.Logic.Licensing
{
  public class Caching<T> where T : class
  {
    private static readonly MemoryCacheEntryOptions _cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(12.0)).SetPriority(CacheItemPriority.NeverRemove);
    private const int hoursCached = 12;

    public static T CacheGet(IMemoryCache cache, string keyEntry)
    {
      T obj;
      return cache.TryGetValue<T>((object) keyEntry, out obj) ? obj : default (T);
    }

    public static void CacheSet(IMemoryCache cache, string keyEntry, T item)
    {
      cache.Set<T>((object) keyEntry, item, IdentityServer4.Admin.Logic.Logic.Licensing.Caching<T>._cacheEntryOptions);
    }

    public static void CacheRemove(IMemoryCache cache, string keyEntry)
    {
      cache.Remove((object) keyEntry);
    }

    public static void CacheRefresh(IMemoryCache cache, T item, string keyEntry)
    {
      T obj1;
      if (cache.TryGetValue<T>((object) keyEntry, out obj1))
        IdentityServer4.Admin.Logic.Logic.Licensing.Caching<T>.CacheRemove(cache, keyEntry);
      T obj2 = item;
      cache.Set<T>((object) keyEntry, obj2, IdentityServer4.Admin.Logic.Logic.Licensing.Caching<T>._cacheEntryOptions);
    }
  }
}
