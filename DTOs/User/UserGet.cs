namespace PennyPlanner.DTOs.User
{
    public class UserGet
    {
        public int Id { get; set; }
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public bool GetNotifications { get; set; } = false;
        public string? Name { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
