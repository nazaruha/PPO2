using FluentValidation;
using PPO2.Core.DTOs.ProductDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.Validation.Product
{
    public class ProductCreateValidation : AbstractValidator<ProductCreateDto>
    {
        public ProductCreateValidation()
        {
            RuleFor(p => p.Name).NotEmpty().MaximumLength(maximumLength: 50);
            RuleFor(p => p.ManufacturerId).NotEmpty();
        }
    }
}
