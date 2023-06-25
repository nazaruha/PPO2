using FluentValidation;
using PPO2.Core.DTOs.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.Validation.Order
{
    public class OrderUpdateValidation : AbstractValidator<OrderUpdateDto>
    {
        public OrderUpdateValidation()
        {
            RuleFor(o => o.CustomerId).NotEmpty();
            RuleFor(o => o.ProjectId).NotEmpty();
            RuleFor(o => o.ProductId).NotEmpty();
            RuleFor(o => o.TotalPrice).NotEmpty();
            RuleFor(o => o.ProductQuantity).NotEmpty();
            RuleFor(o => o.SellDate).NotEmpty();
        }
    }
}
