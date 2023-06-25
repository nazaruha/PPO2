using FluentValidation;
using PPO2.Core.DTOs.ProjectDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.Validation.Project
{
    public class ProjectUpdateValidation : AbstractValidator<ProjectUpdateDto>   
    {
        public ProjectUpdateValidation()
        {
            RuleFor(p => p.Id).NotEmpty();
            RuleFor(p => p.Name).NotEmpty().MaximumLength(maximumLength: 50);
        }
    }
}
