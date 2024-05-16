using PennyPlanner.Enums;

namespace PennyPlanner.Models
{
    public class Transaction : BaseEntity
    {
        public User User { get; set; } = default!;
        public Account Account { get; set; } = default!;
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public Periodicity Periodicity { get; set; } = Periodicity.None;
        public TransactionType TransactionType { get; set; }
        public string? Description { get; set; }
    }
}
