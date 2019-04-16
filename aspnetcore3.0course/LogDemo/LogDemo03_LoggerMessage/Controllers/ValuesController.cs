using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LogDemo03_LoggerMessage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly ILogger<ValuesController> _logger;
        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }
               
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            _logger.IndexPageRequested("----------get--------------");
            return new string[] { "value1", "value2" };
        }


        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            for (int i = 1; i < 5; i++)
            {
                _logger.DeleteLog(id+i);
            }
            return "value";
        }     
    }

    public static class LogExt
    {
        public static void IndexPageRequested(this ILogger logger, string message)
        {

            _indexPageRequested(logger, message, null);
        }
        private static readonly Action<ILogger, string, Exception> _indexPageRequested = LoggerMessage.Define<string>(
                 LogLevel.Information,
                 new EventId(1),
                 "自定义日志消息:{0}");

        public static void DeleteLog(this ILogger logger, int id)
        {
            DeleteScope(logger, id);
        }
        private static Func<ILogger, int, IDisposable> DeleteScope = LoggerMessage.DefineScope<int>("删除了{id}");

    }
}
