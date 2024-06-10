namespace PennyPlanner.DTOs.User
{
    public class UserCreate
    {
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string? Name { get; set; }
    }
}
