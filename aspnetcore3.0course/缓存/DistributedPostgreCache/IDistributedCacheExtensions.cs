using Microsoft.Extensions.Caching.Distributed;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DistributedPostgreCache
{
    public static class IDistributedCacheExtensions
    {
        const int seconds = 20;
        public static byte[] GetOrCreate(this IDistributedCache distributedCache, string key, Func<CacheItem, byte[]> factory)
        {
            var connectionString = (distributedCache as DistributedPostgreCache).ConnectionString;
            var tableName = (distributedCache as DistributedPostgreCache).TableName;
            using (var con = new NpgsqlConnection(connectionString))
            {
                con.Open();
                var cmd = new NpgsqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"select id,value,expiresattime,slidingexpirationinseconds,absoluteexpiration from {tableName} where id=@id";
                cmd.Parameters.Add(new NpgsqlParameter("id", key));
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    var cacheItem = new CacheItem();
                    cacheItem.Id = reader.GetFieldValue<string>(Columns.Indexes.CacheItemIdIndex);
                    cacheItem.Value = reader.GetFieldValue<byte[]>(Columns.Indexes.CacheItemValueIndex);
                    var expiresAtTime = reader.GetFieldValue<DateTimeOffset?>(Columns.Indexes.ExpiresAtTimeIndex);
                    cacheItem.SlidingExpirationInSeconds = reader.GetFieldValue<TimeSpan?>(Columns.Indexes.SlidingExpirationInSecondsIndex);
                    cacheItem.AbsoluteExpiration = reader.GetFieldValue<DateTimeOffset?>(Columns.Indexes.AbsoluteExpirationIndex);
                    if (cacheItem.AbsoluteExpiration.HasValue)
                    {
                        if (expiresAtTime > DateTimeOffset.UtcNow)
                        {
                            return cacheItem.Value;
                        }
                        else
                        {
                            return SetCatch(key, distributedCache, factory);
                        }
                    }
                    else
                    {
                        if (expiresAtTime - DateTimeOffset.UtcNow > TimeSpan.Zero)
                        {
                            var entry = new CacheItem();
                            var value = factory(entry);
                            if (!entry.AbsoluteExpiration.HasValue && !entry.SlidingExpirationInSeconds.HasValue)
                            {
                                entry.SlidingExpirationInSeconds = TimeSpan.FromSeconds(seconds);
                            }
                            if (!entry.SlidingExpirationInSeconds.HasValue && entry.AbsoluteExpiration.HasValue)
                            {
                                entry.SlidingExpirationInSeconds = entry.AbsoluteExpiration.Value.UtcDateTime - DateTimeOffset.UtcNow;
                            }
                            distributedCache.Set(key, cacheItem.Value, new DistributedCacheEntryOptions { AbsoluteExpiration = entry.AbsoluteExpiration, SlidingExpiration = entry.SlidingExpirationInSeconds, AbsoluteExpirationRelativeToNow = entry.SlidingExpirationInSeconds });
                            return cacheItem.Value;
                        }
                        else
                        {
                            return SetCatch(key, distributedCache, factory);
                        }
                    }
                }
                else
                {
                    return SetCatch(key, distributedCache, factory);
                }
            }
        }



        static byte[] SetCatch(string key, IDistributedCache distributedCache, Func<CacheItem, byte[]> factory)
        {
            var entry = new CacheItem();
            var value = factory(entry);
            if (!entry.AbsoluteExpiration.HasValue && !entry.SlidingExpirationInSeconds.HasValue)
            {
                entry.SlidingExpirationInSeconds = TimeSpan.FromSeconds(seconds);
            }
            if (!entry.SlidingExpirationInSeconds.HasValue && entry.AbsoluteExpiration.HasValue)
            {
                entry.SlidingExpirationInSeconds = entry.AbsoluteExpiration.Value.UtcDateTime - DateTimeOffset.UtcNow;
            }
            distributedCache.Set(key, value, new DistributedCacheEntryOptions { AbsoluteExpiration = entry.AbsoluteExpiration, SlidingExpiration = entry.SlidingExpirationInSeconds, AbsoluteExpirationRelativeToNow = entry.SlidingExpirationInSeconds });
            return value;
        }


        public static async Task<byte[]> GetOrCreateAsync(this IDistributedCache distributedCache, string key, Func<CacheItem, byte[]> factory, CancellationToken token = default)
        {
            var connectionString = (distributedCache as DistributedPostgreCache).ConnectionString;
            var tableName = (distributedCache as DistributedPostgreCache).TableName;
            using (var con = new NpgsqlConnection(connectionString))
            {
                await con.OpenAsync();
                var cmd = new NpgsqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"select id,value,expiresattime,slidingexpirationinseconds,absoluteexpiration from {tableName} where id=@id";
                cmd.Parameters.Add(new NpgsqlParameter("id", key));
                var reader = await cmd.ExecuteReaderAsync(token);
                if (await reader.ReadAsync())
                {
                    var cacheItem = new CacheItem();
                    cacheItem.Id = reader.GetFieldValue<string>(Columns.Indexes.CacheItemIdIndex);
                    cacheItem.Value = reader.GetFieldValue<byte[]>(Columns.Indexes.CacheItemValueIndex);
                    var expiresAtTime = reader.GetFieldValue<DateTimeOffset?>(Columns.Indexes.ExpiresAtTimeIndex);
                    cacheItem.SlidingExpirationInSeconds = reader.GetFieldValue<TimeSpan?>(Columns.Indexes.SlidingExpirationInSecondsIndex);
                    cacheItem.AbsoluteExpiration = reader.GetFieldValue<DateTimeOffset?>(Columns.Indexes.AbsoluteExpirationIndex);
                    if (cacheItem.AbsoluteExpiration.HasValue)
                    {
                        if (expiresAtTime > DateTimeOffset.UtcNow)
                        {
                            return cacheItem.Value;
                        }
                        else
                        {
                            return await SetCatchAsync(key, distributedCache, factory, token);
                        }
                    }
                    else
                    {
                        if (expiresAtTime - DateTimeOffset.UtcNow > TimeSpan.Zero)
                        {
                            var entry = new CacheItem();
                            var value = factory(entry);
                            if (!entry.AbsoluteExpiration.HasValue && !entry.SlidingExpirationInSeconds.HasValue)
                            {
                                entry.SlidingExpirationInSeconds = TimeSpan.FromSeconds(seconds);
                            }
                            if (!entry.SlidingExpirationInSeconds.HasValue && entry.AbsoluteExpiration.HasValue)
                            {
                                entry.SlidingExpirationInSeconds = entry.AbsoluteExpiration.Value.UtcDateTime - DateTimeOffset.UtcNow;
                            }
                            await distributedCache.SetAsync(key, cacheItem.Value, new DistributedCacheEntryOptions { AbsoluteExpiration = entry.AbsoluteExpiration, SlidingExpiration = entry.SlidingExpirationInSeconds, AbsoluteExpirationRelativeToNow = entry.SlidingExpirationInSeconds }, token);
                            return cacheItem.Value;
                        }
                        else
                        {
                            return await SetCatchAsync(key, distributedCache, factory, token);
                        }
                    }
                }
                else
                {
                    return await SetCatchAsync(key, distributedCache, factory, token);
                }
            }
        }

        static async Task<byte[]> SetCatchAsync(string key, IDistributedCache distributedCache, Func<CacheItem, byte[]> factory, CancellationToken token = default)
        {
            var entry = new CacheItem();
            var value = factory(entry);
            if (!entry.AbsoluteExpiration.HasValue && !entry.SlidingExpirationInSeconds.HasValue)
            {
                entry.SlidingExpirationInSeconds = TimeSpan.FromSeconds(seconds);
            }
            if (!entry.SlidingExpirationInSeconds.HasValue && entry.AbsoluteExpiration.HasValue)
            {
                entry.SlidingExpirationInSeconds = entry.AbsoluteExpiration.Value.UtcDateTime - DateTimeOffset.UtcNow;
            }
            await distributedCache.SetAsync(key, value, new DistributedCacheEntryOptions { AbsoluteExpiration = entry.AbsoluteExpiration, SlidingExpiration = entry.SlidingExpirationInSeconds, AbsoluteExpirationRelativeToNow = entry.SlidingExpirationInSeconds }, token);
            return value;
        }

    }
}
