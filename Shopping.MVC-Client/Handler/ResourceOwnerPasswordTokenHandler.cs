using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Shopping.MVC_Client.Exceptions;
using IAuthenticationService = Shopping.Core.Services.IAuthenticationService;

namespace Shopping.MVC_Client.Handler
{
    public class ResourceOwnerPasswordTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IAuthenticationService authenticationService;

        public ResourceOwnerPasswordTokenHandler(IHttpContextAccessor contextAccessor, IAuthenticationService authenticationService)
        {
            this.contextAccessor = contextAccessor;
            this.authenticationService = authenticationService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await this.contextAccessor.HttpContext.GetTokenAsync
                (OpenIdConnectParameterNames.AccessToken);

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue
                ("Bearer", accessToken);

            var response = await base.SendAsync(request, cancellationToken);

            if (response is { StatusCode: System.Net.HttpStatusCode.Unauthorized })
            {
                var tokenResponse = await this.authenticationService.GetAccessTokenByRefreshTokenAsync();

                if (tokenResponse != null)
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue
                        ("Bearer", tokenResponse.AccessToken);

                    response = await base.SendAsync(request, cancellationToken);
                }
            }

            if (response is { StatusCode: System.Net.HttpStatusCode.Unauthorized })
                throw new UnAuthorizeException();

            return response;
        }
    }
}
