namespace PennyPlanner.Models
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public bool GetNotifications { get; set; } = false;
        public string? Name { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
