using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HealthChecksDemo02
{
    /// <summary>
    /// 数据始化的健康检查
    /// </summary>
    public class DataBaseInitServiceHealthCheck : IHealthCheck
    {
        /// <summary>
        /// 
        /// </summary>
        private volatile bool _startupTaskCompleted = false;

        public string Name => "slow_dependency_check";

        public bool StartupTaskCompleted
        {
            get => _startupTaskCompleted;
            set => _startupTaskCompleted = value;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (StartupTaskCompleted)
            {
                return Task.FromResult(HealthCheckResult.Healthy("数据库初始化开始"));
            }
            return Task.FromResult(HealthCheckResult.Unhealthy("数据库初始化中……"));
        }
    }
}
