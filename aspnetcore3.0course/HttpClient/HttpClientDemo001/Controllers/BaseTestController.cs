using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HttpClientDemo001.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseTestController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public BaseTestController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet("testget")]
        public async Task<ActionResult> TestGet()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/values");
            request.Headers.Add("User-Agent", "HttpClientDemo001");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Content(result);
            }
            return null;
        }
        [HttpGet("testgetid")]
        public async Task<ActionResult> TestGetID()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/values/1");
            request.Headers.Add("User-Agent", "HttpClientDemo001");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Content(result);
            }
            return null;
        }
        [HttpGet("testgetidname")]
        public async Task<ActionResult> TestGetIDName()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/values/idname?id=222&name=enterty222");
            request.Headers.Add("User-Agent", "HttpClientDemo001");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Content(result);
            }
            return null;
        }
        [HttpGet("testpost")]
        public async Task<ActionResult> TestPost()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/api/values");
            request.Content = new StringContent("{\"id\":1,\"name\":\"实体1\"}", Encoding.UTF8, "application/json");
            request.Headers.Add("User-Agent", "HttpClientDemo001");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Content(result);
            }
            return null;
        }

        [HttpGet("testput")]
        public async Task<ActionResult> TestPut()
        {
            var request = new HttpRequestMessage(HttpMethod.Put, "http://localhost:5000/api/values/1");
            request.Content = new StringContent("{\"id\":1,\"name\":\"实体1\"}", Encoding.UTF8, "application/json");
            request.Headers.Add("User-Agent", "HttpClientDemo001");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Content(result);
            }
            return null;
        }

        [HttpGet("testdelete")]
        public async Task<ActionResult> TestDelete()
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, "http://localhost:5000/api/values/1");        
            request.Headers.Add("User-Agent", "HttpClientDemo001");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Content(result);
            }
            return null;
        }

    }
}
