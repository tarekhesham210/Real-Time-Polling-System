using Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterRequestDTO>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name Is Required")
                .Length(3, 10).WithMessage("First Name Must be More Than 3 Letters And Less Than 10 Letters");
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name Is Required")
                .Length(3, 10).WithMessage("Last Name Must be More Than 3 Letters And Less Than 10 Letters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email Is Required")
                .EmailAddress().WithMessage("InValid Email");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password Is Required")
                .MinimumLength(8).WithMessage("Password must be at least 8 letters")
                .Matches(@"[A-Z]").WithMessage("Password must has at least Capital Letter")
                .Matches(@"[a-z]").WithMessage("Password must has at least small Letter")
                .Matches(@"[0-9]").WithMessage("Password must has at least one number")
                .Matches(@"[\!\?\*\.\@\#\$\&]").WithMessage("Password must has at least one special character(!?*.@#$&)");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Confirm password must match password");
        }
    }
}
