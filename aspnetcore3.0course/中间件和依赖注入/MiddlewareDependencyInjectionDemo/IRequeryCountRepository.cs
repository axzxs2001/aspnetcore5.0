using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddlewareDependencyInjectionDemo
{
    /// <summary>
    /// 请求记录仓储接口
    /// </summary>
    public interface IRequeryCountRepository
    {
        string ID { get; }
        Dictionary<string, bool> RequestCount { get; set; }
    }
}
