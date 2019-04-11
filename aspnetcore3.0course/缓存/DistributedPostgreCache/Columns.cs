using System;

namespace DistributedPostgreCache
{
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
