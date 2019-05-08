using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FilterDemo02.Filters;
namespace FilterDemo02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        [MyFilter("桂素伟", "MyFilter", Order = 1)]
        [MyFilterFactory]
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            Console.WriteLine("=========================get===========================");
            return new string[] { "value1", "value2" };
        }
        [TypeFilter(typeof(MyResourceFilter))]
        [ServiceFilter(typeof(MyExceptionFilter))]
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            if(id==2)
            {
                throw new Exception("这里有异常");
            }
            return "value";
        }


        [HttpPost]
        public void Post([FromBody] string value)
        {
        }


        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }


        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
