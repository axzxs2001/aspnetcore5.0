using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MiddlewareDependencyInjectionDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly IRequeryCountRepository _requeryCountRepository;
        public ValuesController(IRequeryCountRepository requeryCountRepository)
        {
            _requeryCountRepository = requeryCountRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {            
            return new string[] { "Get", $"请求总次数：{_requeryCountRepository.RequestCount.Count.ToString()},正在处理请求：{_requeryCountRepository.RequestCount.Count(d => d.Value) - 1} ,ID：{_requeryCountRepository.ID}" };
        }


        [HttpGet("{id}")]
        public ActionResult<string> Get([FromServices]IRequeryCountRepository requeryCountRepository, int id)
        {
            if (id == 1)
            {
                throw new Exception("id=1异常");
            }
            else
            {
                System.Threading.Thread.Sleep(2000);
            }
            return $"GetID，请求总次数：{requeryCountRepository.RequestCount.Count.ToString()},正在处理请求：{requeryCountRepository.RequestCount.Count(d => d.Value) - 1} ,ID：{requeryCountRepository.ID}";
        }

    }
}
