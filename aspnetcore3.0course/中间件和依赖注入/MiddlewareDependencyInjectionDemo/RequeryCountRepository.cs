using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddlewareDependencyInjectionDemo
{
    /// <summary>
    /// 请求记录仓储
    /// </summary>
    public class RequeryCountRepository : IRequeryCountRepository
    {
        public RequeryCountRepository()
        {
            Console.WriteLine(DateTime.Now);
        }
        string _id;
        public string ID
        {
            get
            {
                if (string.IsNullOrEmpty(_id))
                {
                    _id = Guid.NewGuid().ToString();
                }
                return _id;
            }
        }
        public Dictionary<string, bool> RequestCount { get; set; } = new Dictionary<string, bool>();
    }
}
