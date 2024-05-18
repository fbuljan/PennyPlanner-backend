using FluentValidation;
using PennyPlanner.DTOs.User;
using PennyPlanner.Utils;

namespace PennyPlanner.Validation
{
    public class UserCreateValidator : AbstractValidator<UserCreate>
    {
        public UserCreateValidator()
        {
            RuleFor(userCreate => userCreate.Email).NotEmpty().EmailAddress().MaximumLength(50);
            RuleFor(userCreate => userCreate.Username).NotEmpty().MaximumLength(30);
            RuleFor(userCreate => userCreate.Password).NotEmpty().MinimumLength(8)
                .Must(ValidatePassword).WithMessage("Password must contain at least one number, one lowercase letter, and one uppercase letter.");
        }

        private bool ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            return PasswordUtils.ValidatePassword(password);
        }
    }
}
