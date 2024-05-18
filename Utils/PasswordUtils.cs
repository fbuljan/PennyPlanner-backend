using System.Text.RegularExpressions;

namespace PennyPlanner.Utils
{
    public static class PasswordUtils
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public static bool ValidatePassword(string password)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasUpperChar = new Regex(@"[A-Z]+");

            return hasNumber.IsMatch(password) && hasLowerChar.IsMatch(password) && hasUpperChar.IsMatch(password);
        }
    }
}
