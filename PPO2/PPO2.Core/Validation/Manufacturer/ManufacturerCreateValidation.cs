using FluentValidation;
using PPO2.Core.DTOs.ManufacturerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.Validation.Manufacturer
{
    public class ManufacturerCreateValidation : AbstractValidator<ManufacturerCreateDto>
    {
        public ManufacturerCreateValidation()
        {
            RuleFor(m => m.Name).NotEmpty().MaximumLength(maximumLength: 50);
        }
    }
}
