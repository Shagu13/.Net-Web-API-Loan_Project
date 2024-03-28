using FluentValidation;
using Loan_Api_Project.Models.DTO;
using System.Text.RegularExpressions;

namespace Loan_Api.Validation
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(user => user.FirstName)
               .NotEmpty().WithMessage("FirstName field must not be empty.")
               .Length(1, 50).WithMessage("Firstname must be between 1 and 50 characters.");

            RuleFor(user => user.LastName)
                .NotEmpty().WithMessage("LastName field must not be empty.")
                .Length(1, 50).WithMessage("Lastname must be between 1 and 50 characters.");

            RuleFor(user => user.UserName)
                .NotEmpty().WithMessage("UserName field must not be empty.")
                .Length(1, 50).WithMessage("Username must be between 1 and 50 characters.")
                .Matches("^[a-zA-Z0-9]*$").WithMessage("Username can only contain letters or digits.");

            RuleFor(user => user.Age)
                .NotEmpty().WithMessage("Age field must not be empty.");

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress()
                .MaximumLength(200).WithMessage("Email must not exceed 200 characters.");
                

            RuleFor(user => user.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(50).WithMessage("Password must not exceed 50 characters.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$")
                .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
        }

        private bool BeValidEmailFormat(string email)
        {
            string pattern = @"^\S+@\S+\.\S+$";
            return Regex.IsMatch(email, pattern);
        }

        private bool BeValidEmailContent(string email)
        {
            return !string.IsNullOrWhiteSpace(email);
        }
    }
}
