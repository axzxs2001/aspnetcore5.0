using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using DistributedPostgreCache;

namespace CacheDemo01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistributedController : ControllerBase
    {
        private IDistributedCache _cache;

        public DistributedController(IDistributedCache distributedCache)
        {
            _cache = distributedCache;
        }
        public string CachedTimeUTC { get; set; }

        [HttpGet("/get")]
        public async Task<string> GetTime()
        {
            CachedTimeUTC = "Cached Time Expired";
            var encodedCachedTimeUTC = await _cache.GetAsync("cachedTimeUTC");

            if (encodedCachedTimeUTC != null)
            {
                CachedTimeUTC = Encoding.UTF8.GetString(encodedCachedTimeUTC);
                return CachedTimeUTC;
            }
            else
            {
                return "";
            }
        }
        [HttpGet("/set")]
        public IActionResult SetTime()
        {
            var currentTimeUTC = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
            byte[] encodedCurrentTimeUTC = Encoding.UTF8.GetBytes(currentTimeUTC);
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(10))
                .SetSlidingExpiration(TimeSpan.FromSeconds(10))
                ;
            _cache.Set("cachedTimeUTC", encodedCurrentTimeUTC, options);

            return Ok();
        }

        [HttpGet("/delete")]
        public async Task<IActionResult> DeleteTime()
        {
            await _cache.RemoveAsync("cachedTimeUTC");
            return Ok();
        }
        [HttpGet("/getorcreate")]
        public async Task<IActionResult> GetOrCreate()
        {            
            //获取，如果不存在或过期就创建
            var result = await _cache.GetOrCreateAsync("cachedTimeUTC", cacheitem =>
              {
                  cacheitem.AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(10);
               
                  var currentTimeUTC = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
                  var encodedCurrentTimeUTC = Encoding.UTF8.GetBytes(currentTimeUTC);
                  cacheitem.Value = encodedCurrentTimeUTC;

                  return encodedCurrentTimeUTC;
              });
            var cachedTimeUTC = Encoding.UTF8.GetString(result);
            return Ok(cachedTimeUTC);
        }
    }


}
