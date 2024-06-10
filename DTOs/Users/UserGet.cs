using PennyPlanner.Models;

namespace PennyPlanner.DTOs.User
{
    public class UserGet
    {
        public int Id { get; set; }
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string? Name { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<Account> Accounts { get; set; } = default!;
        public List<Transaction> Transactions { get; set; } = default!;
        public List<Goal> Goals { get; set; } = default!;
    }
}
