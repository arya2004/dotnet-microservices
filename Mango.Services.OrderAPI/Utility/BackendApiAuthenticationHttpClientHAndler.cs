using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
namespace Mango.Services.OrderAPI.Utility
{
    public class BackendApiAuthenticationHttpClientHAndler : DelegatingHandler
    { 
        private readonly IHttpContextAccessor _contextAccessor;
        public BackendApiAuthenticationHttpClientHAndler(IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var Tokem = await _contextAccessor.HttpContext.GetTokenAsync("access_token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Tokem);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
