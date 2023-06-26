using Shopping.Core.Models;
using Shopping.Core.Services;
using System.Net.Http.Json;

namespace Shopping.Service.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _client;

        public UserService(HttpClient client)
        {
            _client = client;
        }

        public async Task<UserViewModel> GetUserAsync()
        {
            return await _client.GetFromJsonAsync<UserViewModel>("/api/user/getuser");
        }
    }
}
