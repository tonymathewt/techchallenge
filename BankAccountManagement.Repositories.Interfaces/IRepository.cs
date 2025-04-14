using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManagement.Repositories.Interfaces
{
    public interface IRepository<TEntity, TContext>
    {
        Task<IQueryable<TEntity>> GetAllAsync();

 
        Task<IQueryable<TResult>> GetWhereAsync<TResult>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> selector,
            params string[] includeNavigationProperties);

   
        Task<IQueryable<TEntity>> GetWhereAsync(
            Expression<Func<TEntity, bool>> predicate);
    }

}
