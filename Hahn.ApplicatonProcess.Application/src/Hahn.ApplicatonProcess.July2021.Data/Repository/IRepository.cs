using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.July2021.Data.Repository
{
    public interface IRepository<T> where T : class, new() 
    {
        Task<T> GetByIdAsync(object id);

        IEnumerable<T> GetAll(bool eager = false);

        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        Task<T> AddAsync(T entity);

        T Update(T entity);

        void Delete(T entity);

    }
}
