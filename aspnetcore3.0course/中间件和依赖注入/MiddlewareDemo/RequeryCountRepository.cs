using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddlewareDemo
{
    /// <summary>
    /// 请求记录仓储
    /// </summary>
    public class RequeryCountRepository : IRequeryCountRepository
    {
        public Dictionary<string, bool> RequestCount { get; set; } = new Dictionary<string, bool>();
    }
}
