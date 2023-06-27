using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SharedLibrary.Dtos;
using Shopping.Core.Models.JWTDbModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shopping.Service.Services
{
    public class AuthenticationService : Shopping.Core.Services.IAuthenticationService
    {
        private readonly HttpClient httpClient;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ClientSettings clientSettings;
        private readonly ServiceApiSettings serviceApiSettings;
        private readonly IClientAccessTokenCache clientAccessTokenCache;

        public AuthenticationService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor,
            IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings, IClientAccessTokenCache clientAccessTokenCache)
        {
            this.httpClient = httpClient;
            this.httpContextAccessor = httpContextAccessor;
            this.clientSettings = clientSettings.Value;
            this.serviceApiSettings = serviceApiSettings.Value;
            this.clientAccessTokenCache = clientAccessTokenCache;
        }

        public async Task<TokenResponse> GetAccessTokenByRefreshTokenAsync()
        {
            var disco = await this.httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = this.serviceApiSettings.IdentityBaseUri,
            });

            if (disco is { IsError: true })
                throw disco.Exception;

            var refreshToken = await this.httpContextAccessor.HttpContext.GetTokenAsync
                (OpenIdConnectParameterNames.RefreshToken);

            RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest()
            {
                ClientId = this.clientSettings.WebClientForUser.ClientId,
                ClientSecret = this.clientSettings.WebClientForUser.ClientSecret,
                RefreshToken = refreshToken,
                Address = disco.TokenEndpoint
            };

            var token = await this.httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

            if (token is { IsError: true })
                return null;

            var authenticationTokens = new List<AuthenticationToken>()
            {
                new AuthenticationToken{ Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
                new AuthenticationToken{ Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},
                new AuthenticationToken{ Name=OpenIdConnectParameterNames.ExpiresIn,
                    Value= DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}
            };

            var authenticationResult = await this.httpContextAccessor.HttpContext.AuthenticateAsync();
            var properties = authenticationResult.Properties;
            properties.StoreTokens(authenticationTokens);

            await this.httpContextAccessor.HttpContext.SignInAsync
                (CookieAuthenticationDefaults.AuthenticationScheme, authenticationResult.Principal, properties);

            return token;
        }

        public async Task GeTRefreshTokenAsync()
        {
            var disco = await this.httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = this.serviceApiSettings.IdentityBaseUri,
            });

            if (disco is { IsError: true })
                throw disco.Exception;

            var refreshToken = await this.httpContextAccessor.HttpContext.GetTokenAsync
            (OpenIdConnectParameterNames.RefreshToken);

            RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest()
            {
                ClientId = this.clientSettings.WebClientForUser.ClientId,
                ClientSecret = this.clientSettings.WebClientForUser.ClientSecret,
                Address = disco.RevocationEndpoint,
                RefreshToken = refreshToken,
            };

            var token = await this.httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

            if (token is { IsError: true })
                throw token.Exception;

            var tokens = new List<AuthenticationToken>()
            {

                new AuthenticationToken{ Name=OpenIdConnectParameterNames.IdToken,Value= token.IdentityToken},
                new AuthenticationToken{ Name=OpenIdConnectParameterNames.AccessToken,Value= token.AccessToken},
                new AuthenticationToken{ Name=OpenIdConnectParameterNames.RefreshToken,Value= token.RefreshToken},
                new AuthenticationToken{ Name=OpenIdConnectParameterNames.ExpiresIn,
                    Value= DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o", CultureInfo.InvariantCulture)}
            };

            var authenticationResult = await this.httpContextAccessor.HttpContext.AuthenticateAsync();
            var properties = authenticationResult.Properties;
            properties.StoreTokens(tokens);

            await this.httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                authenticationResult.Principal, properties);
        }

        public async Task<string> GetTokenByClientAsync()
        {
            var currentToken = await this.clientAccessTokenCache.GetAsync("WebClientToken", new ClientAccessTokenParameters());

            if (currentToken != null)
                return currentToken.AccessToken;

            var disco = await this.httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = this.serviceApiSettings.IdentityBaseUri,
            });

            if (disco.IsError)
                throw disco.Exception;

            var clientCredentialTokenRequest = new ClientCredentialsTokenRequest
            {
                ClientId = this.clientSettings.WebClient.ClientId,
                ClientSecret = this.clientSettings.WebClient.ClientSecret,
                Address = disco.TokenEndpoint
            };

            var newToken = await this.httpClient.RequestClientCredentialsTokenAsync(clientCredentialTokenRequest);

            if (newToken.IsError)
                throw newToken.Exception;

            await this.clientAccessTokenCache.SetAsync
                ("WebClientToken", newToken.AccessToken, newToken.ExpiresIn, default);

            return newToken.AccessToken;
        }

        public async Task RevokeRefreshTokenAsync()
        {
            var disco = await this.httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = this.serviceApiSettings.IdentityBaseUri,
            });

            if (disco is { IsError: true })
                throw disco.Exception;

            var refreshToken = await this.httpContextAccessor.HttpContext.GetTokenAsync
                (OpenIdConnectParameterNames.RefreshToken);

            TokenRevocationRequest tokenRevocationRequest = new TokenRevocationRequest()
            {
                ClientId = this.clientSettings.WebClientForUser.ClientId,
                ClientSecret = this.clientSettings.WebClientForUser.ClientSecret,
                Address = disco.RevocationEndpoint,
                Token = refreshToken,
                TokenTypeHint = "refresh_token"
            };

            await this.httpClient.RevokeTokenAsync(tokenRevocationRequest);
        }

        public async Task<Response<bool>> SignInAsync(SignInInput signInInput)
        {
            var disco = await this.httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = this.serviceApiSettings.IdentityBaseUri,
            });

            if (disco is { IsError: true })
                throw disco.Exception;

            var passwordTokenRequest = new PasswordTokenRequest
            {
                ClientId = this.clientSettings.WebClientForUser.ClientId,
                ClientSecret = this.clientSettings.WebClientForUser.ClientSecret,
                UserName = signInInput.Email,
                Password = signInInput.Password,
                Address = disco.TokenEndpoint
            };

            var token = await this.httpClient.RequestPasswordTokenAsync(passwordTokenRequest);

            if (token is { IsError: true })
            {
                var responseContent = await token.HttpResponse.Content.ReadAsStringAsync();
                var errorDto = JsonSerializer.Deserialize<ErrorDto>
                    (responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return Response<bool>.Fail(errorDto.Errors, 400);
            }

            var userInfoRequest = new UserInfoRequest
            {
                Token = token.AccessToken,
                Address = disco.UserInfoEndpoint
            };

            var userInfo = await this.httpClient.GetUserInfoAsync(userInfoRequest);

            if (userInfo is { IsError: true })
                throw userInfo.Exception;

            ClaimsIdentity claimsIdentity = new ClaimsIdentity
                (userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authenticationProperties = new AuthenticationProperties();

            authenticationProperties.StoreTokens(new List<AuthenticationToken>
            {
                new AuthenticationToken{Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
                new AuthenticationToken{Name= OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},
                  new AuthenticationToken{ Name=OpenIdConnectParameterNames.ExpiresIn,
                    Value= DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}
            });

            authenticationProperties.IsPersistent = signInInput.IsRemember;

            await this.httpContextAccessor.HttpContext.SignInAsync
                (CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);

            return Response<bool>.Success(200);
        }
    }
}
