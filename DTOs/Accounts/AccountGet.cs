using PennyPlanner.Models;

namespace PennyPlanner.DTOs.Accounts
{
    public class AccountGet
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public float Balance { get; set; }
        public string? Description { get; set; }
        public List<Transaction> Transactions { get; set; } = default!;
    }
}
