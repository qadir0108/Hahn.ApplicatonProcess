using Hahn.ApplicatonProcess.July2021.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Hahn.ApplicatonProcess.July2021.Data.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly HahnContext _context;
        public UserRepository(HahnContext context) : base(context)
        {
            _context = context;
        }
    }
}
