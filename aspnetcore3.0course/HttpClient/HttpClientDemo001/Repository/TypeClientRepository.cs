using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientDemo001.Repository
{
    public class TypeClientRepository : ITypeClientRepository
    {
        readonly HttpClient _client;
        public TypeClientRepository(HttpClient client)
        {
            client.BaseAddress = new Uri("http://localhost:5000/");
            client.DefaultRequestHeaders.Add("User-Agent", "HttpClientDemo001");
            _client = client;
        }
        public async Task<List<Entity>> GetEntities()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/values");           
            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Entity>>(result);
            }
            return null;
        }
    }
}
