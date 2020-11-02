using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogDemo03_LoggerMessage
{
    /// <summary>
    /// 日志扩展类
    /// </summary>
    public static class LoggerExtension
    {
        /// <summary>
        /// 写标题log
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        public static void LogTitle(this ILogger logger, string message)
        {
            _logTitle(logger, message, null);
        }
        private static readonly Action<ILogger, string, Exception> _logTitle = LoggerMessage.Define<string>(
                 LogLevel.Information,
                 new EventId(1),
                 "自定义日志消息:{0}");

        /// <summary>
        /// 删除log
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="id"></param>
        public static void DeleteLog(this ILogger logger, int id)
        {
            DeleteScope(logger, id);
        }
        private static Func<ILogger, int, IDisposable> DeleteScope = LoggerMessage.DefineScope<int>("删除了{id}");

    }
}
