using TaskFlowPro.Shared.Dtos;

namespace TaskFlowPro.Client.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersAsync();
    }
}
