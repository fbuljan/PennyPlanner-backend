using PennyPlanner.DTOs.User;

namespace PennyPlanner.Services.Interfaces
{
    public interface IAuthService
    {
        bool VerifyPassword(string inputPassword, string storedPassword);
        string GenerateJwtToken(UserGet user);
    }
}
