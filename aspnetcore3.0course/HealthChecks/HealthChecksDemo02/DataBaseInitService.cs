using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HealthChecksDemo02
{
    /// <summary>
    /// 数据库初始化服务
    /// </summary>
    public class DataBaseInitService : IHostedService, IDisposable
    {
        /// <summary>
        /// 延迟秒数
        /// </summary>
        private readonly int _delaySeconds = 15;
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger _logger;
        /// <summary>
        /// 数据始化的健康检查
        /// </summary>
        private readonly DataBaseInitServiceHealthCheck _startupHostedServiceHealthCheck;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="logger">日志</param>
        /// <param name="startupHostedServiceHealthCheck">数据始化的健康检查</param>
        public DataBaseInitService(ILogger<DataBaseInitService> logger, DataBaseInitServiceHealthCheck startupHostedServiceHealthCheck)
        {
            _logger = logger;
            _startupHostedServiceHealthCheck = startupHostedServiceHealthCheck;
        }
        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"数据库初始化开始……");
            Task.Run(async () =>
            {
                //执行初始化需要的时间
                await Task.Delay(_delaySeconds * 1000);

                _startupHostedServiceHealthCheck.StartupTaskCompleted = true;

                _logger.LogInformation($"数据库初始化完成");
            });
            return Task.CompletedTask;
        }
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("数据库初始化停止");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}
