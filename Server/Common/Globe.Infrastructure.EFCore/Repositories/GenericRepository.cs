using Globe.BusinessLogic.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Globe.Infrastructure.EFCore.Repositories
{
    public class GenericRepository<TContext, TEntity> : IRepository<TEntity>
        where TContext : DbContext
        where TEntity : class
    {
        readonly protected TContext _context;

        private DbSet<TEntity> DbSet { get { return this._context.Set<TEntity>(); } }

        public GenericRepository(TContext context)
        {
            _context = context;
        }

        public virtual IQueryable<TEntity> Query(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>,
                IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return query;
        }

        virtual public IEnumerable<TEntity> Get(
                    Expression<Func<TEntity, bool>> filter = null,
                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = this.DbSet.AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                return orderBy(query).ToList();

            return query.ToList();
        }

        virtual public TEntity FindById(Guid id)
        {
            return _context.Find<TEntity>(id);
        }

        virtual public void Insert(TEntity entity)
        {
            this.DbSet.Add(entity);
        }

        virtual public void Update(TEntity entity)
        {
            this.DbSet.Update(entity);
        }

        virtual public void Delete(TEntity entity)
        {
            this.DbSet.Remove(entity);
        }
    }

    public class AsyncGenericRepository<TContext, TEntity> : IAsyncRepository<TEntity>
        where TContext : DbContext
        where TEntity : class
    {
        readonly protected TContext _context;

        private DbSet<TEntity> DbSet { get { return this._context.Set<TEntity>(); } }

        public AsyncGenericRepository(TContext context)
        {
            _context = context;
        }

        public virtual Task<IQueryable<TEntity>> QueryAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>,
                IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return Task.FromResult(query);
        }

        async virtual public Task<IEnumerable<TEntity>> GetAsync(
                    Expression<Func<TEntity, bool>> filter = null,
                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = this.DbSet.AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                return orderBy(query).ToList();

            return await query.ToListAsync();
        }

        async virtual public Task<TEntity> FindByIdAsync(Guid id)
        {
            return await _context.FindAsync<TEntity>(id);
        }

        async virtual public Task InsertAsync(TEntity entity)
        {
            await this.DbSet.AddAsync(entity);
        }

        async virtual public Task UpdateAsync(TEntity entity)
        {
            await Task.Run(() => this.DbSet.Update(entity));
        }

        async virtual public Task DeleteAsync(TEntity entity)
        {
            await Task.Run(() => this.DbSet.Remove(entity));
        }
    }
}
