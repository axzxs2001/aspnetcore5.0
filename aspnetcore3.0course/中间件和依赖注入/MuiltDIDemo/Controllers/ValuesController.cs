using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MuiltDIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly IDBService _postgreDBService;
        readonly IDBService _sqlServerDBService;
        public ValuesController(IEnumerable<IDBService> dBServices)
        {
            foreach (var dBService in dBServices)
            {
                switch (dBService.DBType)
                {
                    case DBType.PostgreSql:
                        _postgreDBService = dBService;
                        break;
                    case DBType.SqlServer:
                        _sqlServerDBService = dBService;
                        break;
                }
            }
        }


        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            if (id == 1)
            {
                return _postgreDBService.ConnectionString;
            }
            else
            {
                return _sqlServerDBService.ConnectionString;
            }    
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
