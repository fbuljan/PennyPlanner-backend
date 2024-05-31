using FluentValidation;
using PennyPlanner.DTOs.Transactions;

namespace PennyPlanner.Validation
{
    public class TransactionUpdateValidator : AbstractValidator<TransactionUpdate>
    {
        public TransactionUpdateValidator()
        {
            RuleFor(transactionUpdate => transactionUpdate.Amount).GreaterThan(0);
        }
    }
}
