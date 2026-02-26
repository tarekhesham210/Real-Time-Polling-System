using Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class VoteValidator:AbstractValidator<VoteRequest>
    {
        public VoteValidator()
        {
            RuleFor(v=>v.pollId).GreaterThan(0).WithMessage("Invalid Choice");
            RuleFor(v=>v.optionId).GreaterThan(0).WithMessage("Invalid Choice");
        }
    }
}
