using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;
using System.Data;

namespace CacheDemo01
{
    /// <summary>
    /// Distributed cache implementation using Microsoft SQL Server database.
    /// </summary>
    public class PostgreCache : IDistributedCache
    {
        readonly PostgreCacheOptions _options;

        public PostgreCache(IOptions<PostgreCacheOptions> options)
        {
            _options = options.Value;
        }
        public byte[] Get(string key)
        {
            using (var con = new NpgsqlConnection(_options.ConnectionString))
            {
                con.Open();
                var cmd = new NpgsqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"select id,value,expiresattime,slidingexpirationinseconds,absoluteexpiration from @tablename where id=@id";
                cmd.Parameters.Add(new NpgsqlParameter("tablename", _options.TableName));
                cmd.Parameters.Add(new NpgsqlParameter("id", key));
                var reader = cmd.ExecuteReader(
                    CommandBehavior.SequentialAccess | CommandBehavior.SingleRow | CommandBehavior.SingleResult);
                if (reader.Read())
                {
                    return reader.GetFieldValue<byte[]>(Columns.Indexes.CacheItemValueIndex);
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<byte[]> GetAsync(string key, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            using (var con = new NpgsqlConnection(_options.ConnectionString))
            {
                await con.OpenAsync();
                var cmd = new NpgsqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"select value from @tablename where id=@id";
                cmd.Parameters.Add(new NpgsqlParameter("tablename", _options.TableName));
                cmd.Parameters.Add(new NpgsqlParameter("id", key));
                var reader = await cmd.ExecuteReaderAsync();
                if (reader.Read())
                {
                    return await reader.GetFieldValueAsync<byte[]>(Columns.Indexes.CacheItemValueIndex, token);
                }
                else
                {
                    return null;
                }
            }

        }

        public void Refresh(string key)
        {
            Get(key);
        }

        public async Task RefreshAsync(string key, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            await GetAsync(key);
        }

        public void Remove(string key)
        {
            using (var con = new NpgsqlConnection(_options.ConnectionString))
            {
                con.Open();
                var cmd = new NpgsqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"delete from @tablename where id=@id";
                cmd.Parameters.Add(new NpgsqlParameter("tablename", _options.TableName));
                cmd.Parameters.Add(new NpgsqlParameter("id", key));
                cmd.ExecuteNonQuery();
            }
        }

        public async Task RemoveAsync(string key, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            using (var con = new NpgsqlConnection(_options.ConnectionString))
            {
                await con.OpenAsync();
                var cmd = new NpgsqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"delete from @tablename where id=@id";
                cmd.Parameters.Add(new NpgsqlParameter("tablename", _options.TableName));
                cmd.Parameters.Add(new NpgsqlParameter("id", key));
                await cmd.ExecuteNonQueryAsync(token);
            }
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            DateTimeOffset utcNow = new SystemClock().UtcNow;

            var absoluteExpiration = GetAbsoluteExpiration(utcNow, options);
            ValidateOptions(options.SlidingExpiration, absoluteExpiration);

            using (var con = new NpgsqlConnection(_options.ConnectionString))
            {
                con.Open();
                var cmd = new NpgsqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"select id,value,expiresattime,slidingexpirationinseconds,absoluteexpiration from @tablename where id=@id";
                cmd.Parameters.Add(new NpgsqlParameter("tablename", _options.TableName));
                cmd.Parameters.Add(new NpgsqlParameter("id", key));
                cmd.ExecuteNonQuery();
             
            }
        }

        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        protected DateTimeOffset? GetAbsoluteExpiration(DateTimeOffset utcNow, DistributedCacheEntryOptions options)
        {
            // calculate absolute expiration
            DateTimeOffset? absoluteExpiration = null;
            if (options.AbsoluteExpirationRelativeToNow.HasValue)
            {
                absoluteExpiration = utcNow.Add(options.AbsoluteExpirationRelativeToNow.Value);
            }
            else if (options.AbsoluteExpiration.HasValue)
            {
                if (options.AbsoluteExpiration.Value <= utcNow)
                {
                    throw new InvalidOperationException("The absolute expiration value must be in the future.");
                }

                absoluteExpiration = options.AbsoluteExpiration.Value;
            }
            return absoluteExpiration;
        }

        protected void ValidateOptions(TimeSpan? slidingExpiration, DateTimeOffset? absoluteExpiration)
        {
            if (!slidingExpiration.HasValue && !absoluteExpiration.HasValue)
            {
                throw new InvalidOperationException("Either absolute or sliding expiration needs " +
                    "to be provided.");
            }
        }
    }

    public static class PostgreCachingServicesExtensions
    {
        public static IServiceCollection AddDistributedPostgreCache(this IServiceCollection services, Action<PostgreCacheOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            services.AddOptions();
            AddPostgreCacheServices(services);
            services.Configure(setupAction);

            return services;
        }

        internal static void AddPostgreCacheServices(IServiceCollection services)
        {
            services.Add(ServiceDescriptor.Singleton<IDistributedCache, PostgreCache>());
        }
    }

    public class PostgreCacheOptions : IOptions<PostgreCacheOptions>
    {

        public ISystemClock SystemClock { get; set; }


        public TimeSpan? ExpiredItemsDeletionInterval { get; set; }


        public string ConnectionString { get; set; }


        public string SchemaName { get; set; }


        public string TableName { get; set; }


        public TimeSpan DefaultSlidingExpiration { get; set; } = TimeSpan.FromMinutes(20);

        PostgreCacheOptions IOptions<PostgreCacheOptions>.Value
        {
            get
            {
                return this;
            }
        }
    }
    internal static class Columns
    {
        public static class Names
        {
            public const string CacheItemId = "Id";
            public const string CacheItemValue = "Value";
            public const string ExpiresAtTime = "ExpiresAtTime";
            public const string SlidingExpirationInSeconds = "SlidingExpirationInSeconds";
            public const string AbsoluteExpiration = "AbsoluteExpiration";
        }

        public static class Indexes
        {
            public const int CacheItemIdIndex = 0;
            public const int CacheItemValueIndex = 1;
            public const int ExpiresAtTimeIndex = 2;
            public const int SlidingExpirationInSecondsIndex = 3;
            public const int AbsoluteExpirationIndex = 4;
        }
    }
}
