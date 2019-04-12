using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedPostgreCache
{
    public class PostgreCacheOptions : IOptions<PostgreCacheOptions>
    {

        public string ConnectionString { get; set; }  

        public string TableName { get; set; }

        PostgreCacheOptions IOptions<PostgreCacheOptions>.Value
        {
            get
            {
                return this;
            }
        }
    }
}
