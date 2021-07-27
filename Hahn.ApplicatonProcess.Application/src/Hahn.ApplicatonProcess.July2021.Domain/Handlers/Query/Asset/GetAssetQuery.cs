using System.Collections.Generic;
using Hahn.ApplicatonProcess.July2021.Data.Remote;
using Hahn.ApplicatonProcess.July2021.Domain.Models;
using MediatR;

namespace Hahn.ApplicatonProcess.July2021.Domain.Query
{
    public class GetAssetQuery : IRequest<List<RemoteAsset>>
    {
        public int UserId { get; set; }
    }
}