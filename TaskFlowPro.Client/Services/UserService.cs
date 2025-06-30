using System.Net.Http.Json;
using TaskFlowPro.Shared.Dtos;

namespace TaskFlowPro.Client.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<UserDto>>("api/users");
        }
    }
}
