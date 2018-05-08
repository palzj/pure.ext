#if NETSTANDARD2_0
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq.Expressions;
/// <summary>
/// 系统缓存拓展类
/// </summary>
public static class FromCacheExtensions
{

    static MemoryCache cacheDefault = new MemoryCache(new MemoryCacheOptions());


    private const string CACHE_PRIFIX = "Pure.Ext.Caching;";
    /// <summary>
    /// 根据Key 缓存对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="this"></param>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static TValue FromCache<T, TValue>(this T @this, MemoryCache cache, string key, TValue value)
    {
        object item = cache.GetOrCreate(key, (k) => { return value; });

        return (TValue)item;
    }

    /// <summary>
    /// 根据Key 缓存对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="this"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static TValue FromCache<T, TValue>(this T @this, string key, TValue value)
    {
        return @this.FromCache(cacheDefault, key, value);
    }

    /// <summary>A TKey extension method that from cache.</summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <typeparam name="TValue">Type of the value.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cache">The cache.</param>
    /// <param name="key">The key.</param>
    /// <param name="valueFactory">The value factory.</param>
    /// <returns>A TValue.</returns>
    public static TValue FromCache<T, TValue>(this T @this, MemoryCache cache, string key, Expression<Func<T, TValue>> valueFactory)
    {
        var lazy = new Lazy<TValue>(() => valueFactory.Compile()(@this));
        Lazy<TValue> item = (Lazy<TValue>)cache.GetOrCreate(key, (k) => { return lazy; });
        return item.Value;
    }

    /// <summary>
    /// 根据Key 缓存对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="this"></param>
    /// <param name="key"></param>
    /// <param name="valueFactory"></param>
    /// <returns></returns>
    public static TValue FromCache<T, TValue>(this T @this, string key, Expression<Func<T, TValue>> valueFactory)
    {
        return @this.FromCache(cacheDefault, key, valueFactory);
    }

    /// <summary>
    /// 根据Key 缓存对象
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="this"></param>
    /// <param name="valueFactory"></param>
    /// <returns></returns>
    public static TValue FromCache<TKey, TValue>(this TKey @this, Expression<Func<TKey, TValue>> valueFactory)
    {
        string key = string.Concat(CACHE_PRIFIX, typeof(TKey).FullName, valueFactory.ToString());
        return @this.FromCache(cacheDefault, key, valueFactory);
    }

    /// <summary>
    /// 根据Key 缓存对象
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="this"></param>
    /// <param name="cache"></param>
    /// <param name="valueFactory"></param>
    /// <returns></returns>
    public static TValue FromCache<TKey, TValue>(this TKey @this, MemoryCache cache, Expression<Func<TKey, TValue>> valueFactory)
    {
        string key = string.Concat(CACHE_PRIFIX, typeof(TKey).FullName, valueFactory.ToString());
        return @this.FromCache(cache, key, valueFactory);
    }




    /// <summary>
    /// 获取或者新增对象缓存
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static TValue AddOrGetExisting<TValue>(this MemoryCache cache, string key, TValue value)
    {
        object val = cache.GetOrCreate(key, (k) => { return value; });
        return (TValue)val;


    }

    /// <summary>
    /// 获取或者新增对象缓存
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <param name="valueFactory"></param>
    /// <returns></returns>
    public static TValue AddOrGetExisting<TValue>(this MemoryCache cache, string key, Func<string, TValue> valueFactory)
    {
        var lazy = new Lazy<TValue>(() => valueFactory(key));

        Lazy<TValue> item = (Lazy<TValue>)cache.GetOrCreate(key, (k) => { return lazy; });

        return item.Value;
    }

    /// <summary>
    /// 获取或者新增对象缓存
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <param name="valueFactory"></param>
    /// <param name="absoluteExpiration"></param>
    /// <returns></returns>
    public static TValue AddOrGetExisting<TValue>(this MemoryCache cache, string key, Func<string, TValue> valueFactory, DateTimeOffset absoluteExpiration)
    {
        var lazy = new Lazy<TValue>(() => valueFactory(key));

        Lazy<TValue> item = (Lazy<TValue>)cache.GetOrCreate(key, (k) => {
            k.AbsoluteExpiration = absoluteExpiration;
            return lazy;
        });

        return item.Value;
    }

}


#endif
