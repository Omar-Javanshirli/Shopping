﻿
using IdentityModel.Client;
using SharedLibrary.Dtos;

namespace Shopping.Core.Services
{
    public interface IAuthenticationService
    {
        Task<TokenResponse> GetAccessTokenByRefreshToken();
        Task RevokeRefreshToken();
        Task GetTokenByClient();
        Task GeTRefreshToken();
        Task<Response<bool>> SignIn(SignInInput signInInput);
    }
}
