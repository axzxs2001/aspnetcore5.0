using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ConfigrationDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly Appsetting _appsetting;
        public ValuesController(IOptionsSnapshot<Appsetting> optionsSnapshot)
        {
            _appsetting = optionsSnapshot.Value;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", $"Key:{ _appsetting.Key},Name:{_appsetting.Name}" };

        }


        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            //写配置文件的不要
            if (id == 1)
            {
                var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Data.json");
                var jsonContext = System.IO.File.ReadAllText(jsonFile);
                var data = JsonConvert.DeserializeObject<dynamic>(jsonContext);
                return Convert.ToString(data.Data1);
            }
            else
            {
                var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Data.json");
                var jsonContext = System.IO.File.ReadAllText(jsonFile);
                var data = JsonConvert.DeserializeObject<dynamic>(jsonContext);
                data.Data1 = data.Data1 + DateTime.Now.ToString();
                jsonContext = JsonConvert.SerializeObject(data);
                System.IO.File.WriteAllText(jsonFile, jsonContext);
                return "成功写入：" + Convert.ToString(data.Data1);
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
