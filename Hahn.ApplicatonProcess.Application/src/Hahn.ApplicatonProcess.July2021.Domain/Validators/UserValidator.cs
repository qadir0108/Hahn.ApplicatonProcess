using FluentValidation;
using Hahn.ApplicatonProcess.July2021.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.July2021.Domain.Validators
{
    public class UserValidator : AbstractValidator<UserVm>
    {
        public UserValidator()
        {
            RuleFor(x => x.Age).Cascade(CascadeMode.Stop).GreaterThan(18).WithMessage("{PropertyName} must be at greater then 18");
            RuleFor(x => x.FirstName).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("{PropertyName} is required.").MinimumLength(3).WithMessage("{PropertyName} must be at least 3 characters");
            RuleFor(x => x.LastName).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("{PropertyName} is required.").MinimumLength(3).WithMessage("{PropertyName} must be at least 3 characters");
            //RuleFor(x => x.Email).Cascade(CascadeMode.Stop).EmailAddress().WithMessage("Please enter valid email address");
            RuleFor(x => x.Email).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("{PropertyName} is required.").Matches(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$").WithMessage("Please enter valid email address");
            RuleFor(x => x.Address).Cascade(CascadeMode.Stop).NotNull().WithMessage("{PropertyName} is required.").SetValidator(new AddressValidator());
        }
    }
}
