using FluentValidation;
using Hahn.ApplicatonProcess.July2021.Data.Entities;
using MediatR;

namespace Hahn.ApplicatonProcess.July2021.Domain.Command
{
    public class DeleteUserCommand : IRequest<User>
    {
        public int Id { get; set; }
    }

    public class DeleteUserQueryValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserQueryValidator()
        {
            RuleFor(a => a.Id).GreaterThan(0).WithMessage("Please pass correct User Id ");
        }
    }
}