using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hahn.ApplicatonProcess.July2021.Data.Entities;
using Hahn.ApplicatonProcess.July2021.Data.UnitOfWoork;
using Hahn.ApplicatonProcess.July2021.Domain.Models;
using MediatR;

namespace Hahn.ApplicatonProcess.July2021.Domain.Query
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User>
    {
        private readonly IUnitOfWork _uow;

        public GetUserByIdQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var users = _uow.UsersRepository.GetAll(true);
            var user = users.SingleOrDefault(x => x.Id == request.Id);
            if (user == null)
                throw new System.Exception($"User with id: {request.Id} not found.");

            return user;
        }
    }
}