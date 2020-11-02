using Microsoft.Extensions.Logging;
using System;

namespace LogLevelTest
{
    public interface ITestLog
    {
        void Log();


    }
    public class TestLog : ITestLog
    {
        private readonly ILogger<TestLog> _logger;
        public TestLog(ILogger<TestLog> logger)
        {
            _logger = logger;
        }
        public void Log()
        {
            _logger.LogTrace("***********  TestLog.Log Trace");
            _logger.LogWarning("***********  TestLog.Log Warning");
            _logger.LogInformation("***********  TestLog.Log Information");
            _logger.LogCritical("***********  TestLog.Log Critical");
            _logger.LogDebug("***********  TestLog.Log Debug");
            _logger.LogError("***********  TestLog.Log Error");
        }
    }
}
