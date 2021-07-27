using Hahn.ApplicatonProcess.July2021.Data.Entities;
using MediatR;

namespace Hahn.ApplicatonProcess.July2021.Domain.Command
{
    public class CreateUserCommand : IRequest<User>
    {
        public User User { get; set; }
    }
}