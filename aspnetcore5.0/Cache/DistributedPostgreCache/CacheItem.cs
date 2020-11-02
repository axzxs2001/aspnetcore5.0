using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedPostgreCache
{
    public class CacheItem
    {
        public string Id { get; set; }
        public byte[] Value { get; set; }

        public TimeSpan? SlidingExpirationInSeconds { get; set; }
        public DateTimeOffset? AbsoluteExpiration { get; set; }
    }
}
