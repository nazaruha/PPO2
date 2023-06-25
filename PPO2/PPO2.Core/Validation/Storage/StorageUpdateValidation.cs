using FluentValidation;
using PPO2.Core.DTOs.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.Validation.Storage
{
    public class StorageUpdateValidation : AbstractValidator<StorageUpdateDto>
    {
        public StorageUpdateValidation()
        {
            RuleFor(s => s.ProjectId).NotEmpty();
            RuleFor(s => s.ProductId).NotEmpty();
            RuleFor(s => s.Price).NotEmpty(); 
            RuleFor(s => s.Count).NotEmpty();   
            RuleFor(s => s.Description).MaximumLength(maximumLength: 255);
            RuleFor(s => s.ExpireDate).NotEmpty();
        }
    }
}
