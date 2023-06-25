using FluentValidation;
using PPO2.Core.DTOs.ProjectDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.Validation.Project
{
    public class ProjectCreateValidation : AbstractValidator<ProjectCreateDto>
    {
        public ProjectCreateValidation()
        {
            RuleFor(p => p.Name).NotEmpty().MaximumLength(maximumLength: 50);
        }
    }
}
