using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hahn.ApplicatonProcess.July2021.Data.Entities;
using Hahn.ApplicatonProcess.July2021.Data.UnitOfWoork;
using Hahn.ApplicatonProcess.July2021.Domain.Models;
using MediatR;

namespace Hahn.ApplicatonProcess.July2021.Domain.Command
{
    public class TrackAssetCommandHandler : IRequestHandler<TrackAssetCommand, int>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public TrackAssetCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<int> Handle(TrackAssetCommand request, CancellationToken cancellationToken)
        {
            var users = _uow.UsersRepository.GetAll(true);
            var user = users.SingleOrDefault(x => x.Id == request.UserId);
            if (user == null)
                throw new System.Exception($"User with id: {request.UserId} not found.");

            var asset = await GetAsset(request.AssetId);
            if (request.TrackUntrack)
            {
                user.Assets.Add(asset);
            } else
            {
                user.Assets.Remove(asset);
            }
            _uow.UsersRepository.Update(user);
            return await _uow.SaveChangesAsync();
        }

        private async Task<Asset> GetAsset(string assetId)
        {
            var asset = await _uow.AssetsRepository.GetByIdAsync(assetId);
            if (asset == null)
            {
                var remoteAsset = _uow.RemoteAssetsRepository.GetAll().SingleOrDefault(x => x.id == assetId);
                if (remoteAsset == null) throw new System.Exception($"Remote Asset with id: {assetId} not found.");
                await _uow.AssetsRepository.AddAsync(new Asset() { Id = remoteAsset.id, Symbol = remoteAsset.symbol, Name = remoteAsset.name });
                await _uow.SaveChangesAsync();
                asset = await _uow.AssetsRepository.GetByIdAsync(assetId);
            }
            return asset;
        }
    }
}