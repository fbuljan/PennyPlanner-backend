using FluentValidation;
using PennyPlanner.DTOs.Goals;

namespace PennyPlanner.Validation
{
    public class GoalCreateValidator : AbstractValidator<GoalCreate>
    {
        public GoalCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.TargetValue).GreaterThanOrEqualTo(0);
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
