using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HealthChecksDemo01
{ 
    /// <summary>
    /// postgre健康检查
    /// </summary>
    public class PostgreHealthCheck : IHealthCheck
    {
        private readonly IDbConnection _dbConnection;
        private readonly IConfiguration _configuration;

        public PostgreHealthCheck(IDbConnection dbConnection, IConfiguration configuration)
        {
            _dbConnection = dbConnection;
            _configuration = configuration;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default(CancellationToken))
        {

            var healthCheckResultHealthy = true;
            using (_dbConnection)
            {
                _dbConnection.ConnectionString = _configuration.GetConnectionString("Postgre");
                try
                {
                    _dbConnection.Open();
                }
                catch
                {
                    healthCheckResultHealthy = false;
                }
                if (healthCheckResultHealthy)
                {
                    return Task.FromResult(
                        HealthCheckResult.Healthy("健康"));
                }
                return Task.FromResult(
                    HealthCheckResult.Unhealthy("不健康"));
            }
        }
    }
}
