using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;
using System.Data;

namespace DistributedPostgreCache
{

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
                cmd.CommandText = $"update {_options.TableName} set value=@value,expiresattime=CASE  WHEN @slidingexpirationinseconds IS NUll THEN @absoluteexpiration ELSE floor(extract(epoch FROM (current_timestamp at time zone 'UTC' +(@slidingexpirationinseconds||'second')::interval - to_date('0000-01-01', 'YYYY-MM-DD')))) END,slidingexpirationinseconds=@slidingexpirationinseconds,absoluteexpiration=@absoluteexpiration where id=@id;insert into {_options.TableName}(id,value,expiresattime,slidingexpirationinseconds,absoluteexpiration) select @id,@value,CASE  WHEN @slidingexpirationinseconds IS NUll THEN @absoluteexpiration ELSE floor(extract(epoch FROM (current_timestamp at time zone 'UTC' +(@slidingexpirationinseconds||'second')::interval - to_date('0000-01-01', 'YYYY-MM-DD')))) END,@slidingexpirationinseconds,@absoluteexpiration where not exists(select 1 from {_options.TableName} where id=@id)";

                cmd.Parameters.Add(new NpgsqlParameter("id", key));
                cmd.Parameters.Add(new NpgsqlParameter("value", value));
                if (options.SlidingExpiration.HasValue)
                {
                    cmd.Parameters.Add(new NpgsqlParameter("slidingexpirationinseconds", options.SlidingExpiration.Value.TotalSeconds));
                }
                else
                {
                    cmd.Parameters.Add(new NpgsqlParameter("slidingexpirationinseconds", DbType.Int64){ Value = DBNull.Value });
                }
                if (absoluteExpiration == null)
                {
                    cmd.Parameters.Add(new NpgsqlParameter("absoluteexpiration", DBNull.Value));
                }
                else
                {
                    cmd.Parameters.Add(new NpgsqlParameter("absoluteexpiration", DbType.Int64){ Value = absoluteExpiration.Value.UtcTicks });
                }
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
                cmd.CommandText = $"update {_options.TableName} set value=@value,expiresattime=CASE  WHEN @slidingexpirationinseconds IS NUll THEN @absoluteexpiration ELSE floor(extract(epoch FROM (current_timestamp at time zone 'UTC' +(@slidingexpirationinseconds||'second')::interval - to_date('0000-01-01', 'YYYY-MM-DD')))) END,slidingexpirationinseconds=@slidingexpirationinseconds,absoluteexpiration=@absoluteexpiration where id=@id;insert into {_options.TableName}(id,value,expiresattime,slidingexpirationinseconds,absoluteexpiration) select @id,@value,CASE  WHEN @slidingexpirationinseconds IS NUll THEN @absoluteexpiration ELSE floor(extract(epoch FROM (current_timestamp at time zone 'UTC' +(@slidingexpirationinseconds||'second')::interval - to_date('0000-01-01', 'YYYY-MM-DD')))) END,@slidingexpirationinseconds,@absoluteexpiration where not exists(select 1 from {_options.TableName} where id=@id)";
                cmd.Parameters.Add(new NpgsqlParameter("id", key));
                cmd.Parameters.Add(new NpgsqlParameter("value", value));
                if (options.SlidingExpiration.HasValue)
                {
                    cmd.Parameters.Add(new NpgsqlParameter("slidingexpirationinseconds", options.SlidingExpiration.Value.TotalSeconds));
                }
                else
                {
                    cmd.Parameters.Add(new NpgsqlParameter("slidingexpirationinseconds", DBNull.Value));
                }
                if (absoluteExpiration == null)
                {
                    cmd.Parameters.Add(new NpgsqlParameter("absoluteexpiration", DBNull.Value));
                }
                else
                {
                    cmd.Parameters.Add(new NpgsqlParameter("absoluteexpiration", absoluteExpiration.Value.UtcDateTime));
                }
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
