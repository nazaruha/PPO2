using FluentValidation;
using PPO2.Core.DTOs.ManufacturerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.Validation.Manufacturer
{
    public class ManufacturerUpdateValidation : AbstractValidator<ManufacturerUpdateDto>
    {
        public ManufacturerUpdateValidation()
        {
            RuleFor(m => m.Id).NotEmpty();
            RuleFor(m => m.Name).NotEmpty().MaximumLength(maximumLength: 50);
        }
    }
}
