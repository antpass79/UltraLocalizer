using Globe.BusinessLogic.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Globe.Infrastructure.EFCore.Repositories
{
    public class GenericRepository<T> : IRepository<T>
        where T : class
    {
        readonly protected DbContext _context;

        private DbSet<T> DbSet { get { return this._context.Set<T>(); } }

        public GenericRepository(DbContext context)
        {
            _context = context;
        }

        virtual public T FindById(Guid id)
        {
            return _context.Find<T>(id);
        }

        virtual public IEnumerable<T> Get(
                    Expression<Func<T, bool>> filter = null,
                    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = this.DbSet.AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                return orderBy(query).ToList();

            return query.ToList();
        }

        virtual public void Insert(T entity)
        {
            this.DbSet.Add(entity);
        }

        virtual public void Update(T entity)
        {
            this.DbSet.Update(entity);
        }

        virtual public void Delete(T entity)
        {
            this.DbSet.Remove(entity);
        }
    }

    public class AsyncGenericRepository<T> : IAsyncRepository<T>
        where T : class
    {
        readonly protected DbContext _context;

        private DbSet<T> DbSet { get { return this._context.Set<T>(); } }

        public AsyncGenericRepository(DbContext context)
        {
            _context = context;
        }

        async virtual public Task<T> FindByIdAsync(Guid id)
        {
            return await _context.FindAsync<T>(id);
        }

        async virtual public Task<IEnumerable<T>> GetAsync(
                    Expression<Func<T, bool>> filter = null,
                    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = this.DbSet.AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                return orderBy(query).ToList();

            return await query.ToListAsync();
        }

        async virtual public Task InsertAsync(T entity)
        {
            await this.DbSet.AddAsync(entity);
        }

        async virtual public Task UpdateAsync(T entity)
        {
            await Task.Run(() => this.DbSet.Update(entity));
        }

        async virtual public Task DeleteAsync(T entity)
        {
            await Task.Run(() => this.DbSet.Remove(entity));
        }
    }
}
