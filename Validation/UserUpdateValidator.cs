using FluentValidation;
using PennyPlanner.DTOs.User;
using PennyPlanner.Utils;

namespace PennyPlanner.Validation
{
    public class UserUpdateValidator : AbstractValidator<UserUpdate>
    {
        public UserUpdateValidator()
        {
            RuleFor(userUpdate => userUpdate.Id).NotEmpty();
            RuleFor(userUpdate => userUpdate.Email).EmailAddress().When(userUpdate => !string.IsNullOrEmpty(userUpdate.Email));
            RuleFor(userUpdate => userUpdate.Password).MinimumLength(8).When(userUpdate => !string.IsNullOrEmpty(userUpdate.Password)).
                Must(ValidatePassword).WithMessage("Password must contain at least one number, one lowercase letter, and one uppercase letter."); ;
        }

        private bool ValidatePassword(string? password)
        {
            if (string.IsNullOrEmpty(password))
                return true;

            return PasswordUtils.ValidatePassword(password);
        }
    }
}
