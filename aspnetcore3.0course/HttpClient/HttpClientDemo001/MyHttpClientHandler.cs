using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HttpClientDemo001
{
    public class MyHttpClientHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("--- MyHttpClientHandler Request");
            var result = await base.SendAsync(request, cancellationToken);
            Console.WriteLine("--- MyHttpClientHandler Response");
            Console.WriteLine("-----------------------------------");
            return result;
        }
    }
}
