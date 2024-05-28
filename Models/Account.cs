namespace PennyPlanner.Models
{
    public class Account : BaseEntity
    {
        public User User { get; set; } = default!;
        public string Name { get; set; } = default!;
        public float Balance { get; set; }
        public string? Description { get; set; }
        public List<Transaction> Transactions { get; set; } = default!;
    }
}
