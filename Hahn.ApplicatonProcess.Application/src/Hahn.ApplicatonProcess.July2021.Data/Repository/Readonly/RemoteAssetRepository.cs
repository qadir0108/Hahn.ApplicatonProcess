using Hahn.ApplicatonProcess.July2021.Data.Conincap;
using Hahn.ApplicatonProcess.July2021.Data.Remote;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.July2021.Data.Repository
{
    public class RemoteAssetRepository : IRemoteAssetRepository
    {
        private readonly IConincapClient conincapClient;

        public RemoteAssetRepository(IConincapClient conincapClient)
        {
            this.conincapClient = conincapClient;
        }

        public IEnumerable<RemoteAsset> GetAll()
        {
            var request = conincapClient.CallAsync<RemoteAssetList>(CoincapService.Url);
            return request.GetAwaiter().GetResult().data;
        }
    }
}
