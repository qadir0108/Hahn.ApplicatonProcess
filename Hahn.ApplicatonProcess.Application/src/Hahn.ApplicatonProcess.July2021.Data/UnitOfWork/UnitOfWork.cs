using Hahn.ApplicatonProcess.July2021.Data.Conincap;
using Hahn.ApplicatonProcess.July2021.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.July2021.Data.UnitOfWoork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HahnContext _context;

        public UnitOfWork(HahnContext context)
        {
            _context = context;
            UsersRepository = new UserRepository(_context);
            AssetsRepository = new AssetRepository(_context);
        }

        public UnitOfWork(HahnContext context, IConincapClient conincapClient)
        {
            _context = context;
            UsersRepository = new UserRepository(_context);
            AssetsRepository = new AssetRepository(_context);
            RemoteAssetsRepository = new RemoteAssetRepository(conincapClient);
        }

        public IUserRepository UsersRepository { get; }
        public IAssetRepository AssetsRepository { get; }
        public IRemoteAssetRepository RemoteAssetsRepository { get; }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
