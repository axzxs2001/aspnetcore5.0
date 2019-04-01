using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddlewareDemo
{
    /// <summary>
    /// 请求记录仓储接口
    /// </summary>
    public interface IRequeryCountRepository
    {
        Dictionary<string, bool> RequestCount { get; set; }
    }
}
