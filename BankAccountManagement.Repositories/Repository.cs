using BankAccountManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BankAccountManagement.Repositories
{
    public class Repository<TEntity, TContext> : IRepository<TEntity, TContext>
     where TContext : DbContext
     where TEntity : class
    {
        protected TContext Context;
        protected DbSet<TEntity> Dbset;

        public Repository(TContext context)
        {
            Context = context;
            Dbset = context.Set<TEntity>();
        }

        public Task<IQueryable<TEntity>> GetAllAsync() => Task.FromResult(Context.Set<TEntity>().AsQueryable());

        public Task<IQueryable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            return Task.FromResult(query);
        }

        public Task<IQueryable<TResult>> GetWhereAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> selector,
            params string[] includeNavigationProperties) =>
                Task.FromResult(GetWhereResult(predicate, selector, includeNavigationProperties));

        public Task<IQueryable<TEntity>> GetWhereAsync(
            Expression<Func<TEntity, bool>> predicate) =>
                Task.FromResult(GetWhereResult(predicate));

        public async Task<bool> SaveAsync()
        {
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var savedRequest = await Context.Set<TEntity>().AddAsync(entity);
            await Context.SaveChangesAsync();
            return savedRequest.Entity;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await Context.Set<TEntity>().AddRangeAsync(entities);
            await Context.SaveChangesAsync();
            return true;
        }

        protected IQueryable<TResult> GetAllResult<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            var queryable = Context.Set<TEntity>();
            return queryable.Select(selector);
        }

        protected IQueryable<TEntity> GetWhereResult(Expression<Func<TEntity, bool>> predicate) =>
            Context.Set<TEntity>().Where(predicate);

        protected IQueryable<TResult> GetWhereResult<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, string[] includeNavigationProperties)
        {
            var queryable = Context.Set<TEntity>().Where(predicate);
            if (includeNavigationProperties != null && includeNavigationProperties.Any())
            {
                foreach (var prop in includeNavigationProperties)
                {
                    var hierarchyProps = prop.Split(".");
                    for (var i = 0; i < hierarchyProps.Length; i++)
                    {
                        var navigationPropertyPath = string.Join(".", hierarchyProps.Take(i + 1));
                        queryable = queryable.Include(navigationPropertyPath);
                    }
                }
            }
            return queryable.Select(selector);
        }
    }
}
