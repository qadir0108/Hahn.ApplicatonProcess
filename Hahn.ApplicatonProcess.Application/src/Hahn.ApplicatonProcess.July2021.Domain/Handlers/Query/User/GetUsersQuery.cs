using System.Collections.Generic;
using Hahn.ApplicatonProcess.July2021.Data.Entities;
using MediatR;

namespace Hahn.ApplicatonProcess.July2021.Domain.Query
{
    public class GetUsersQuery : IRequest<List<User>>
    {
    }
}