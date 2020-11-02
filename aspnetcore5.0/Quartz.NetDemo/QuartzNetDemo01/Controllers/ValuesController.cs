using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Quartz;
using System.IO;
using Microsoft.Extensions.FileProviders;
using QuartzNetDemo01.Model.DataModel;
using QuartzNetDemo01.Model;

namespace QuartzNetDemo01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        IBackgroundRepository _backgroundRepository;
        IScheduler _scheduler;
        IOptionsSnapshot<List<CronMethod>> _cronMethod;
     
        public ValuesController(IBackgroundRepository backgroundRepository, IScheduler scheduler, IOptionsSnapshot<List<CronMethod>> cronMethod)
        {
            _scheduler = scheduler;
            _cronMethod = cronMethod;
            _backgroundRepository = backgroundRepository;
          
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value1" };
        }
        [HttpGet("{second}")]
        public ActionResult Get(int second)
        {
            //获取路径方法 
            var path =Directory.GetCurrentDirectory() + "/appsettings.json";
            var json = System.IO.File.ReadAllText(path);

            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json);
            obj.CronJob[0].CronExpression = $"0/{second} * * * * ?";
            _scheduler.Clear().Wait();
            foreach (var cronJob in obj.CronJob)
            {              
                Console.WriteLine($"{cronJob.MethodName},{cronJob.CronExpression}");
                QuartzServicesUtilities.StartJob<BackgroundJob>(_scheduler, cronJob.CronExpression.ToString(), cronJob.MethodName.ToString());
            }
            json = Newtonsoft.Json.JsonConvert.SerializeObject(obj,Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(path, json);
            return Ok();
        }      

    }
}
