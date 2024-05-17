namespace PennyPlanner.Utils
{
    public static class PasswordUtils
    {
        /// <summary>
        /// Generates a hashed password using BCrypt.
        /// </summary>
        /// <param name="password">The plaintext password to hash.</param>
        /// <returns>A hashed version of the input password.</returns>
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Verifies a password against a stored hash.
        /// </summary>
        /// <param name="password">The plaintext password to verify.</param>
        /// <param name="hashedPassword">The hashed password to compare against.</param>
        /// <returns>true if the password matches the hashed password; otherwise, false.</returns>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
