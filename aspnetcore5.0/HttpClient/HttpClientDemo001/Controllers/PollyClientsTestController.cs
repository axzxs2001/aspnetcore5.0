using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HttpClientDemo001.Repository;
using Microsoft.AspNetCore.Mvc;

namespace HttpClientDemo001.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollyClientsTestController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public PollyClientsTestController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }


        [HttpGet("pollyget")]
        public async Task<ActionResult> TestPollyGet()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/values/polly");
            request.Headers.Add("User-Agent", "HttpClientDemo001");
            var client = _clientFactory.CreateClient("pollyclient5000");
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Content(result);
            }
            else
            {
                return Content(response.StatusCode.ToString());
            }
        }
    }
}
