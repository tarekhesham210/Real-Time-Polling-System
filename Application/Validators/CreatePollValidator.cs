using Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class CreatePollValidator : AbstractValidator<CreatePollDTO>
    {
        public CreatePollValidator()
        {
            RuleFor(x => x.Question)
                .NotEmpty().WithMessage("Question is required")
                .MaximumLength(200).WithMessage("Question is too long");

            RuleFor(x => x.Options)
                .NotNull()
                .Must(x => x.Count >= 2).WithMessage("Poll must has at least two Options")
                .Must(x => x.Count <= 10).WithMessage("Options count must not exceed 6");

            RuleForEach(x => x.Options).NotEmpty().WithMessage("Option can not be empty");
        }
    }
}
