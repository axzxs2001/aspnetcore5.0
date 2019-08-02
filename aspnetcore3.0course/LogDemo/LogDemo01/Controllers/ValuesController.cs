using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogLevelTest;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LogDemo01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly ILogger<ValuesController> _logger;
        public ValuesController(ILogger<ValuesController> logger, ITestLog testLog)
        {
            testLog.Log();
            _logger = logger;
        }


        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            //批量Log
            using (_logger.BeginScope("开始作用域"))
            {
                _logger.LogInformation(new EventId(10000), "-----------------Get--------------------");

                System.Threading.Thread.Sleep(5000);
                _logger.LogDebug(new EventId(10002), "-----------------Get--------------------");
                return new string[] { "value1", "value2" };
            }
        }

    }
}
