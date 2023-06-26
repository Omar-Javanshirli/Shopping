using Shopping.Core.Services;
using Shopping.MVC_Client.Exceptions;
using System.Net.Http.Headers;

namespace Shopping.MVC_Client.Handler
{
    public class ClientCredentialTokenHandler : DelegatingHandler
    {
        private readonly IAuthenticationService authenticationService;

        public ClientCredentialTokenHandler(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await this.authenticationService.GetTokenByClientAsync());

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnAuthorizeException();

            return response;
        }
    }
}
