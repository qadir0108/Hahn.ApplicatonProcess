using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.July2021.Data.Repository
{
    public interface IReadonlyRepository<T> where T : class, new() 
    {
        IEnumerable<T> GetAll();

    }
}
