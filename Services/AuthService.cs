using PennyPlanner.DTOs.User;
using PennyPlanner.Services.Interfaces;
using PennyPlanner.Utils;

namespace PennyPlanner.Services
{
    public class AuthService : IAuthService
    {
        private IConfiguration Configuration { get; }

        public AuthService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public bool VerifyPassword(string inputPassword, string storedPassword)
        {
            return PasswordUtils.VerifyPassword(inputPassword, storedPassword);
        }

        public string GenerateJwtToken(UserGet user)
        {
            return AuthUtils.GenerateJwtToken(user, Configuration);
        }
    }
}
