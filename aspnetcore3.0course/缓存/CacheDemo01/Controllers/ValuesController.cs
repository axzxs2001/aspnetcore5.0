using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CacheDemo01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IMemoryCache _cache;

        public ValuesController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {            
            return new string[] { "a", "b" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {        
         
            var cacheEntry = _cache.GetOrCreate(CacheKeys.Entry, entry =>
            {                
                //绝对5s后重新设置
                entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(10));
                //无访问5s后重新设置
                //entry.SlidingExpiration = TimeSpan.FromSeconds(5);
                return  DateTime.Now;
            });
            return "Cache"+cacheEntry.ToString("  yyyy-MM-dd HH:mm:ss.fff");
        }

        // POST api/values
        [HttpGet("/refresh")]
        public void Refresh()
        {
            _cache.Set<DateTime>(CacheKeys.Entry, DateTime.Now);
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

    public static class CacheKeys
    {
        public static string Entry { get { return "_Entry"; } }
        public static string CallbackEntry { get { return "_Callback"; } }
        public static string CallbackMessage { get { return "_CallbackMessage"; } }
        public static string Parent { get { return "_Parent"; } }
        public static string Child { get { return "_Child"; } }
        public static string DependentMessage { get { return "_DependentMessage"; } }
        public static string DependentCTS { get { return "_DependentCTS"; } }
        public static string Ticks { get { return "_Ticks"; } }
        public static string CancelMsg { get { return "_CancelMsg"; } }
        public static string CancelTokenSource { get { return "_CancelTokenSource"; } }
    }
}
