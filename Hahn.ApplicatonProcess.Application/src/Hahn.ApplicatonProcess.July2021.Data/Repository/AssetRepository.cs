using Hahn.ApplicatonProcess.July2021.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Hahn.ApplicatonProcess.July2021.Data.Repository
{
    public class AssetRepository : Repository<Asset>, IAssetRepository
    {
        private readonly HahnContext _context;
        public AssetRepository(HahnContext context) : base(context)
        {
            _context = context;
        }
    }
}
