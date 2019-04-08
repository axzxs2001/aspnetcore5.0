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
    public class NamedClientsTestController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public NamedClientsTestController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet("ncget")]
        public async Task<ActionResult> TestGet()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/values");
            request.Headers.Add("User-Agent", "HttpClientDemo001");
            var client = _clientFactory.CreateClient("nameclient5000");
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
