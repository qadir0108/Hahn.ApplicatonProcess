using Hahn.ApplicatonProcess.July2021.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.July2021.Data.UnitOfWoork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UsersRepository { get; }
        IAssetRepository AssetsRepository { get; }
        IRemoteAssetRepository RemoteAssetsRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
