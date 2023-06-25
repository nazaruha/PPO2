using FluentValidation;
using PPO2.Core.DTOs.PlanDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.Validation.Plan
{
    public class PlanCreateValidation : AbstractValidator<PlanCreateDto>
    {
        public PlanCreateValidation()
        {
            RuleFor(p => p.Text).NotEmpty().MaximumLength(maximumLength: 500);
        }
    }
}
