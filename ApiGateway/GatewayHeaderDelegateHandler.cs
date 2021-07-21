using Microsoft.AspNetCore.Http;
using Ocelot.Middleware;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ApiGateway
{
    public class GatewayHeaderDelegateHandler : DelegatingHandler
    {
        //private readonly IHttpContextAccessor _httpContextAccessor;

        //public GatewayHeaderDelegateHandler(IHttpContextAccessor httpContextAccessor)
        //{
        //    _httpContextAccessor = httpContextAccessor;
        //}
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //IEnumerable<string> headerValues;
            //if (request.Headers.TryGetValues("Authorization", out headerValues))
            //{
            //    string accessToken = headerValues.First();

            //    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Replace("Bearer ", ""));
            //    //request.Headers.Remove("Authorization");
            //}
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
