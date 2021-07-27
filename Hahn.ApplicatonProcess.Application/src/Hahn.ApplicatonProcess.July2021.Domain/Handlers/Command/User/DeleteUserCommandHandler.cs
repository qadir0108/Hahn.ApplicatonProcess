using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hahn.ApplicatonProcess.July2021.Data.Entities;
using Hahn.ApplicatonProcess.July2021.Data.UnitOfWoork;
using Hahn.ApplicatonProcess.July2021.Domain.Models;
using MediatR;

namespace Hahn.ApplicatonProcess.July2021.Domain.Command
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, User>
    {
        private readonly IUnitOfWork _uow;

        public DeleteUserCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<User> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _uow.UsersRepository.GetByIdAsync(request.Id);
            _uow.UsersRepository.Delete(user);
            await _uow.SaveChangesAsync();
            return user;
        }
    }
}