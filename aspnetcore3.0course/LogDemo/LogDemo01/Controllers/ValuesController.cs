using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LogDemo01.Controllers
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
            //批量Log
            using (_logger.BeginScope("开始作用域"))
            {
                _logger.LogInformation(new EventId(10000), "-----------------Get--------------------");


                _logger.LogDebug(new EventId(10002), "-----------------Get--------------------");
                return new string[] { "value1", "value2" };
            }
        }     
    
    }
}
