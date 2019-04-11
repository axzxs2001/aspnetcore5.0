using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedPostgreCache
{
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
}
