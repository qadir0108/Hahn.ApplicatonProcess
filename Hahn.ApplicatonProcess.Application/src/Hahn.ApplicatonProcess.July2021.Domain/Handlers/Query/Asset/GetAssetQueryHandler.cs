using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hahn.ApplicatonProcess.July2021.Data.Remote;
using Hahn.ApplicatonProcess.July2021.Data.UnitOfWoork;
using Hahn.ApplicatonProcess.July2021.Domain.Models;
using MediatR;

namespace Hahn.ApplicatonProcess.July2021.Domain.Query
{
    public class GetAssetQueryHandler : IRequestHandler<GetAssetQuery, List<RemoteAsset>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetAssetQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<RemoteAsset>> Handle(GetAssetQuery request, CancellationToken cancellationToken)
        {
            var remoteAssets = _uow.RemoteAssetsRepository.GetAll();

            var users = _uow.UsersRepository.GetAll(true);
            var user = users.SingleOrDefault(x => x.Id == request.UserId);
            if (user == null)
                throw new System.Exception($"User with id: {request.UserId} not found.");

            return remoteAssets.Where(x => user.Assets.Any(y => y.Id == x.id)).ToList();
        }

    }
}