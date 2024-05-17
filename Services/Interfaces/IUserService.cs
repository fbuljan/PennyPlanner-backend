using PennyPlanner.DTOs.User;

namespace PennyPlanner.Services.Interfaces
{
    public interface IUserService
    {
        Task<int> CreateUserAsync(UserCreate userCreate);
        Task UpdateUserAsync(UserUpdate userUpdate);
        Task DeleteUserAsync(UserDelete userDelete);
        Task<UserGet> GetUserAsync(int id);
        Task<List<UserGet>> GetUsersAsync();
    }
}
