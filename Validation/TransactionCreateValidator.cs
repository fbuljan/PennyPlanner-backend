using FluentValidation;
using PennyPlanner.DTOs.Transactions;

namespace PennyPlanner.Validation
{
    public class TransactionCreateValidator : AbstractValidator<TransactionCreate>
    {
        public TransactionCreateValidator()
        {
            RuleFor(transactionCreate => transactionCreate.Amount).NotEmpty().GreaterThan(0);
        }
    }
}
