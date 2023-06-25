using FluentValidation;
using PPO2.Core.DTOs.CustomerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PPO2.Core.Validation.Customer
{
    public class CustomerUpdateValidation : AbstractValidator<CustomerUpdateDto>
    {
        public CustomerUpdateValidation()
        {
            RuleFor(c => c.Id)
                .NotEmpty();
            RuleFor(c => c.FirstName)
               .NotEmpty()
               .MaximumLength(maximumLength: 50);
            RuleFor(c => c.SecondName)
                .NotEmpty()
                .MaximumLength(maximumLength: 50);
            RuleFor(c => c.Email)
                .NotEmpty()
                .MaximumLength(maximumLength: 150)
                .Must(email => Regex.IsMatch(email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$")) // regex rule for email property
                .WithMessage("Invalid email address"); // error message in case email doesn't correspond to the regex rule
            RuleFor(c => c.Phone)
                .NotEmpty()
                .MaximumLength(maximumLength: 23)
                .Must(phone => Regex.IsMatch(phone, @"^\+?[1-9]\d{1,3}[-.\s]?\(?\d{1,3}\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}$")) // regex rule for phone property
                .WithMessage("Invalid phone number"); // error message in case phone doesn't correspond to the regex rule
        }
    }
}
