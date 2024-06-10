using PennyPlanner.Enums;
using System.Text.Json.Serialization;

namespace PennyPlanner.Models
{
    public class Transaction : BaseEntity
    {
        [JsonIgnore]
        public User User { get; set; } = default!;
        [JsonIgnore]
        public Account Account { get; set; } = default!;
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public TransactionType TransactionType { get; set; }
        public TransactionCategory TransactionCategory { get; set; }
        public string? Description { get; set; }
        public int? OtherAccountId { get; set; }
    }
}
