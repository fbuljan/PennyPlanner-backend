using PennyPlanner.Enums;

namespace PennyPlanner.DTOs.Transactions
{
    public class TransactionCreate
    {
        public int AccountId { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public TransactionType TransactionType { get; set; }
        public TransactionCategory TransactionCategory { get; set; }
        public string? Description { get; set; }
        public int? OtherAccountId { get; set; }
    }
}