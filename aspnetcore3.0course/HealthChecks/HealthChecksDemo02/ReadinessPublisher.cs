using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HealthChecksDemo02
{
    /// <summary>
    /// 健康检查发布器
    /// </summary>
    public class ReadinessPublisher : IHealthCheckPublisher
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger _logger;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="logger"></param>
        public ReadinessPublisher(ILogger<ReadinessPublisher> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 发布项
        /// </summary>
        public List<(HealthReport report, CancellationToken cancellationToken)> Entries
        {
            get;
        } = new List<(HealthReport report, CancellationToken cancellationToken)>();

        public Exception Exception { get; set; }

        /// <summary>
        /// 发布查看健康状态
        /// </summary>
        /// <param name="report">健康报表</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
        {
            Entries.Add((report, cancellationToken));

            _logger.LogInformation("{TIMESTAMP} 准备探针状态: {RESULT}", DateTime.UtcNow, report.Status);
            if (Exception != null)
            {
                throw Exception;
            }

            cancellationToken.ThrowIfCancellationRequested();

            return Task.CompletedTask;
        }
    }
}
