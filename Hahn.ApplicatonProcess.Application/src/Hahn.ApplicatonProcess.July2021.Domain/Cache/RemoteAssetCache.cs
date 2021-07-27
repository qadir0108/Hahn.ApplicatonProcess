using Hahn.ApplicatonProcess.July2021.Data.UnitOfWoork;
using Hahn.ApplicatonProcess.July2021.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.July2021.Domain.Cache
{
    public class RemoteAssetCache : ICache
    {
        private readonly IMemoryCache _cache;
        private readonly IUnitOfWork _uow;

        public RemoteAssetCache(IMemoryCache cache, IUnitOfWork uow)
        {
            this._cache = cache;
            this._uow = uow;
        }

        public T Get<T>(string key) where T : class, new()
        {
            T cached;
            if (!_cache.TryGetValue(key, out cached))
            {
                cached = (T)_uow.RemoteAssetsRepository.GetAll();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(60));
                _cache.Set(key, cached, cacheEntryOptions);
            }
            return cached;
        }
    }
}
