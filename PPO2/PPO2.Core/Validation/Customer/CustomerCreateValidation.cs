using FluentValidation;
using PPO2.Core.DTOs.CustomerDto;
using PPO2.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PPO2.Core.Validation.Customer
{
    public class CustomerCreateValidation : AbstractValidator<CustomerCreateDto>
    {
        public CustomerCreateValidation()
        {
            RuleFor(c => c.FirstName)
                .NotEmpty()
                .MaximumLength(maximumLength: 50);
            RuleFor(c => c.SecondName)
                .NotEmpty()
                .MaximumLength(maximumLength: 50);
            RuleFor(c => c.Email)
                .NotEmpty()
                .MaximumLength(maximumLength: 150)
                .EmailAddress()
                //.Must(email => Regex.IsMatch(email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$")) // regex rule for email property
                .WithMessage("Не вірна пошта"); // error message in case email doesn't correspond to the regex rule
            RuleFor(c => c.Phone)
                .NotEmpty()
                .Length(10, 12)
                .WithMessage("Телефон мусить мати 10-12 цифр")
                .Must(phone => Regex.IsMatch(phone, @"^\d{10,12}$")) // only numbers. legnth [10; 12]
                .WithMessage("Телефон мусить містить тільки цифри довжиною від 10 до 12");
                //.MaximumLength(maximumLength: 23)
                //.Must(phone => Regex.IsMatch(phone, @"^\+?[1-9]\d{1,3}[-.\s]?\(?\d{1,3}\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}$")) // regex rule for phone property
                //.WithMessage("Invalid phone number"); // error message in case phone doesn't correspond to the regex rule
        }
    }
}

// Email Regex explanation
/*
 * `^` and `$` represent the start and end of the string, respectively.
 * `[a-zA-Z0-9_.+-]` matches one or more characters that can be letters (both uppercase and lowercase), digits, underscores, dots, plus signs, or hyphens.
 * `@` matches the literal at symbol
 * `[a-zA-Z0-9-]+` matches one or more characters that can be letters (both uppercase and lowercase), digits, or hyphens, which represent the domain name.
 * `\.` matches the literal dot.
 * `[a-zA-Z0-9-.]+` matches one or more characters that can be letters (both uppercase and lowercase), digits, dots, or hyphens, which represent the top-level domain (e.g., .com, .net, .org). 
 */

//Phone Regex explanation
/*
 * `^` and `$` represent the start and end of the string, respectively.
 * `\+?` allows an optional plus sign at the beginning of the number.
 * `[1-9]\d{1,3}` matches a single non-zero digit followed by one to three digits. This handles the country code portion of the phone number.
 * `[-.\s]?` allows for an optional separator character (dash, dot, or whitespace).
 * `\(?\d{1,3}\)?` matches an optional opening parenthesis, followed by one to three digits, followed by an optional closing parenthesis. This handles the area code portion of the phone number.
 * `[-.\s]?` allows for an optional separator character.
 * `\d{1,4}` matches one to four digits. This handles the main number portion of the phone number.
 * `[-.\s]?` allows for an optional separator character.
 * `\d{1,4}` matches one to four digits. This handles an optional extension or additional number portion of the phone number.
 */