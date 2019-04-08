using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HttpClientDemo001_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        [HttpGet]
        public ActionResult<IEnumerable<Entity>> Get()
        {
            return new Entity[] { new Entity { ID = 1, Name = "实体1" }, new Entity { ID = 2, Name = "实体2" }, new Entity { ID = 3, Name = "实体3" } };
        }


        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return new JsonResult(new Entity { ID = id, Name = $"实体{id}" });
        }
        [HttpGet("idname")]
        public ActionResult<string> IDName(int id, string name)
        {
            return new JsonResult(new Entity { ID = id, Name = name });
        }


        [HttpPost]
        public ActionResult Post([FromBody] Entity enterty)
        {
            enterty.ID = new Random().Next(100, 999);
            Console.WriteLine("******************************************");
            Console.WriteLine($"Post   {enterty.ToString()}");
            Console.WriteLine("******************************************");
            return new JsonResult(enterty);
        }


        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Entity enterty)
        {
            Console.WriteLine("******************************************");
            Console.WriteLine($"Put    {enterty.ToString()}");
            Console.WriteLine("******************************************");
        }


        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Console.WriteLine("******************************************");
            Console.WriteLine($"Delete    {id}");
            Console.WriteLine("******************************************");
        }
    }
}
