using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;
using System.Data;
using NpgsqlTypes;

namespace DistributedPostgreCache
{

    public class DistributedPostgreCache : IDistributedCache
    {
        readonly PostgreCacheOptions _options;


        public string ConnectionString
        {
            get { return _options.ConnectionString; }
        }
        public string TableName
        {
            get { return _options.TableName; }
        }

        public DistributedPostgreCache(IOptions<PostgreCacheOptions> options)
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
                cmd.CommandText = $"select id,value,expiresattime,slidingexpirationinseconds,absoluteexpiration from {_options.TableName} where id=@id";
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
                cmd.CommandText = $"select id,value,expiresattime,slidingexpirationinseconds,absoluteexpiration from {_options.TableName} where id=@id";
                cmd.Parameters.Add(new NpgsqlParameter("id", key));
                var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
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
                cmd.CommandText = $"delete from {_options.TableName} where id=@id";            
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
                cmd.CommandText = $"delete from {_options.TableName} where id=@id";             
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
                cmd.CommandText = $"update {_options.TableName} set value=@value,expiresattime=@expiresattime,slidingexpirationinseconds=@slidingexpirationinseconds,absoluteexpiration=@absoluteexpiration where id=@id;insert into {_options.TableName}(id,value,expiresattime,slidingexpirationinseconds,absoluteexpiration) select @id,@value,@expiresattime,@slidingexpirationinseconds,@absoluteexpiration where not exists(select 1 from {_options.TableName} where id=@id)";

                cmd.Parameters.Add(new NpgsqlParameter("id", key));
                cmd.Parameters.Add(new NpgsqlParameter("value", value));
                if (options.SlidingExpiration.HasValue)
                {
                    cmd.Parameters.Add(new NpgsqlParameter("slidingexpirationinseconds", options.SlidingExpiration.Value));

                }
                else
                {
                    cmd.Parameters.Add(new NpgsqlParameter("slidingexpirationinseconds", NpgsqlDbType.Interval) { Value = DBNull.Value });
                }
                if (options.AbsoluteExpiration.HasValue)
                {
                    cmd.Parameters.Add(new NpgsqlParameter("absoluteexpiration", options.AbsoluteExpiration.Value));
                }
                else
                {
                    cmd.Parameters.Add(new NpgsqlParameter("absoluteexpiration", DBNull.Value));
                }
                cmd.Parameters.Add(new NpgsqlParameter("expiresattime", absoluteExpiration.HasValue ? absoluteExpiration.Value : DateTimeOffset.Now.Add(options.SlidingExpiration.Value)));

                cmd.ExecuteNonQuery();
            }
        }

        public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            DateTimeOffset utcNow = new SystemClock().UtcNow;

            var absoluteExpiration = GetAbsoluteExpiration(utcNow, options);
            ValidateOptions(options.SlidingExpiration, absoluteExpiration);

            using (var con = new NpgsqlConnection(_options.ConnectionString))
            {
                await con.OpenAsync();
                var cmd = new NpgsqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"update {_options.TableName} set value=@value,expiresattime=@expiresattime,slidingexpirationinseconds=@slidingexpirationinseconds,absoluteexpiration=@absoluteexpiration where id=@id;insert into {_options.TableName}(id,value,expiresattime,slidingexpirationinseconds,absoluteexpiration) select @id,@value,@expiresattime,@slidingexpirationinseconds,@absoluteexpiration where not exists(select 1 from {_options.TableName} where id=@id)";

                cmd.Parameters.Add(new NpgsqlParameter("id", key));
                cmd.Parameters.Add(new NpgsqlParameter("value", value));
                if (options.SlidingExpiration.HasValue)
                {
                    cmd.Parameters.Add(new NpgsqlParameter("slidingexpirationinseconds", options.SlidingExpiration.Value));
                }
                else
                {
                    cmd.Parameters.Add(new NpgsqlParameter("slidingexpirationinseconds", NpgsqlDbType.Interval) { Value = DBNull.Value });
                }
                if (options.AbsoluteExpiration.HasValue)
                {
                    cmd.Parameters.Add(new NpgsqlParameter("absoluteexpiration", options.AbsoluteExpiration.Value));
                }
                else
                {
                    cmd.Parameters.Add(new NpgsqlParameter("absoluteexpiration", DBNull.Value));
                }
                cmd.Parameters.Add(new NpgsqlParameter("expiresattime", absoluteExpiration.HasValue ? absoluteExpiration.Value : DateTimeOffset.Now.Add(options.SlidingExpiration.Value)));

                await cmd.ExecuteNonQueryAsync();

            }
        }

        protected DateTimeOffset? GetAbsoluteExpiration(DateTimeOffset utcNow, DistributedCacheEntryOptions options)
        {
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
}
