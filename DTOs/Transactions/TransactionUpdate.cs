﻿using PennyPlanner.Enums;

namespace PennyPlanner.DTOs.Transactions
{
    public class TransactionUpdate
    {
        public int Id { get; set; }
        public int? Amount { get; set; }
        public DateTime? Date { get; set; }
        public TransactionType? TransactionType { get; set; }
        public TransactionCategory? TransactionCategory { get; set; }
        public string? Description { get; set; }
    }
}