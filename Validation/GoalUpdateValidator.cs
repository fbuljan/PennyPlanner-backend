using FluentValidation;
using PennyPlanner.DTOs.Goals;

namespace PennyPlanner.Validation
{
    public class GoalUpdateValidator : AbstractValidator<GoalUpdate>
    {
        public GoalUpdateValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.EndDate)
                .Must(BeAValidDate).WithMessage("End date must be a valid date.")
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage("End date cannot be earlier than today.");
        }

        private bool BeAValidDate(DateTime? date)
        {
            if (!date.HasValue) return true;
            return date.Value > DateTime.MinValue;
        }
    }
}
