using IdentityModel.Client;
using SharedLibrary.Dtos;
using Shopping.Core.Models;
using System.Threading.Tasks;

namespace Shopping.Core.Services
{
    public interface IAuthenticationService
    {
        Task<TokenResponse> GetAccessTokenByRefreshTokenAsync();
        Task RevokeRefreshTokenAsync();
        Task<string> GetTokenByClientAsync();
        Task GeTRefreshTokenAsync();
        Task<Response<bool>> SignInAsync(SignInInput signInInput);
    }
}
