using FluentValidation;
using Hahn.ApplicatonProcess.July2021.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.July2021.Domain.Validators
{
    public class AddressValidator : AbstractValidator<AddressVm>
    {
        public AddressValidator()
        {
            RuleFor(x => x.HouseNumber).Cascade(CascadeMode.Stop).NotNull().WithMessage("{PropertyName} is required.");
            RuleFor(x => x.Street).Cascade(CascadeMode.Stop).NotNull().WithMessage("{PropertyName} is required.");
            RuleFor(x => x.PostalCode).Cascade(CascadeMode.Stop).NotNull().WithMessage("{PropertyName} is required.");
        }
    }
}
