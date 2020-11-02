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
            _logger.LogTitle("----------get--------------");
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
   
}
