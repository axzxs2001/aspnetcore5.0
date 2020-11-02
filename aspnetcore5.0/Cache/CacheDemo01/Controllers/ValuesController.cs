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
        /// <summary>
        /// 缓存
        /// </summary>
        private IMemoryCache _cache;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="memoryCache">缓存</param>
        public ValuesController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }   
        
        [HttpGet("{id}")]
        public ActionResult<string> Get()
        {
            var cacheEntry = _cache.GetOrCreate(CacheKeys.Entry, entry =>
            {
                //绝对5s后重新设置
                entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(5));
                //无访问5s后重新设置
                //entry.SlidingExpiration = TimeSpan.FromSeconds(5);
                return DateTime.Now;
            });
            return "Cache" + cacheEntry.ToString("  yyyy-MM-dd HH:mm:ss.fff");
        }


        [HttpGet("/setvalue")]
        public void SetValue()
        {
            _cache.Set<DateTime>(CacheKeys.Entry, DateTime.Now);
        }
    } 
}
