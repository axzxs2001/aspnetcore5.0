using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedPostgreCache
{
    /// <summary>
    /// postgre缓存配置选项
    /// </summary>
    public class PostgreCacheOptions : IOptions<PostgreCacheOptions>
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>

        public string ConnectionString { get; set; }  
        /// <summary>
        /// 表名
        /// </summary>
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
